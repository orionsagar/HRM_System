using Application.Tasks.Commands.CEmployee;
using Application.Tasks.Queries.QEmployee;
using Application.Tasks.Queries.QEmployeeEducation;
using Application.Tasks.Queries.QEmployeeEmployee;
using Application.Tasks.Queries.QEmployeeExperience;
using UKHRM.Helpers;
using Business.HR;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Rotativa.AspNetCore;
using Application.Tasks.Queries.QOrganisation;
using Application.Tasks.Queries.QEmployeeDocument;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IEmployeeBL _employeeBL;
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeeController(CommonDropdown dropdown, IEmployeeBL employeeBL, IMediator mediator, IGlobalHelper helper, IWebHostEnvironment webHostEnvironment = null)
        {
            _dropdown = dropdown;
            _employeeBL = employeeBL;
            _mediator = mediator;
            _global = helper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> IndexAsync()
        {
            #region Access
            var roleid = _global.GetRoleID();
            var controller = RouteData.Values["controller"];
            var action = RouteData.Values["action"];
            var url = $"{controller}/{action}";
            ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
            ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
            ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
            ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
            #endregion
            var compId = _global.GetCompID();
            //ViewBag.Employees = await _mediator.Send(new GetAllEmployeeQuery() { CompId = compId });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var search = Request.Form["search[value]"].FirstOrDefault();
                var EmpCategoryId_Filter = Convert.ToInt32(Request.Form["EmpCategoryId_Filter"].FirstOrDefault() ?? "0");
                var SectionId_Filter = Convert.ToInt32(Request.Form["SectionId_Filter"].FirstOrDefault() ?? "0");
                var DepartmentId_Filter = Convert.ToInt32(Request.Form["DepartmentId_Filter"].FirstOrDefault() ?? "0");
                var DesignationId_Filter = Convert.ToInt32(Request.Form["DesignationId_Filter"].FirstOrDefault() ?? "0");
                var CardNo_Filter = Request.Form["CardNo_Filter"].FirstOrDefault();
                var EmpName_Filter = Request.Form["EmpName_Filter"].FirstOrDefault();
                var OrgId = _global.GetOrgId();
                var totalrecord = 0;
                var UserId = _global.GetUserID(); ;
                //var Role = DataEncryption.DecryptString(Request.Cookies["Role"]);
                var ComId = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var employees = new List<EmployeeVM>();
                EmployeeFilterVM filterVM = new EmployeeFilterVM()
                {
                    EmpCategoryId_Filter = EmpCategoryId_Filter,
                    SectionId_Filter = SectionId_Filter,
                    DepartmentId_Filter = DepartmentId_Filter,
                    DesignationId_Filter = DesignationId_Filter,
                    CardNo_Filter = CardNo_Filter,
                    EmpName_Filter = EmpName_Filter,
                    OrgId = OrgId
                };

                employees = await _mediator.Send(new GetEmployeeListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = ComId, ClientId = ClientId, EmployeeFilterVM = filterVM });


                if (employees.Count != 0)
                {
                    if (employees.FirstOrDefault().TotalCount != 0)
                    {
                        totalrecord = employees.FirstOrDefault().TotalCount;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = employees });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Access
            var roleid = _global.GetRoleID();
            var controller = RouteData.Values["controller"];
            var action = RouteData.Values["action"];
            var url = $"{controller}/{action}";
            ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
            ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
            ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
            ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
            #endregion
            var compId = _global.GetCompID();
            var orgid = _global.GetOrgId();
            ViewBag.CardNo = await _employeeBL.GenerateCardNo(compId.ToString(), orgid);
            await GetDropdownAsync();
            //await GetEmpTraining(745);
            ViewBag.Action = "Add";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BasicInfoCreate(Employee emp)
        {
            String messages = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (emp.File != null)
                    {
                        string dirFolder = "emp";

                        string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(emp.File.FileName);
                        string fileDire = dirFolder + "/" + fileName;  //Path.Combine(dirFolder, fileName);

                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, fileDire);
                        //string filePathghh = Path.Combine(filePath, emp.File.FileName);

                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        emp.File.CopyTo(fileStream);

                        emp.Photograph = fileDire;
                    }
                    var ClientId = _global.GetClientId();
                    emp.ClientId = ClientId;

                    if (emp.EmployeeHistory.Salary == null)
                    {
                        emp.EmployeeHistory.Salary = 0;
                    }

                    //var status = await _employeeBL.Upsert(emp);
                    var status = await _mediator.Send(new CreateEmployeeCommand { Employee = emp });

                    return Json(status);
                }

                messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage + " " + v.Exception));

                //var erroneousFields = ModelState.Where(ms => ms.Value.Errors.Any())
                //                        .Select(x => new { x.Key, x.Value.Errors }).ToString();

                return Json(new BLStatus { Message = messages, IsError = true });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = messages, IsError = true, StatusCode = "500" });
            }

        }


        [HttpPost]
        public async Task<IActionResult> DetailsInfoCreate(EmployeeDetails emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (emp.ImgFile != null)
                    {
                        string dirFolder = "emp";

                        string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(emp.ImgFile.FileName);
                        string fileDire = Path.Combine(dirFolder, fileName);

                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, fileDire);
                        //string filePathghh = Path.Combine(filePath, emp.File.FileName);

                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        emp.ImgFile.CopyTo(fileStream);

                        emp.ProofOfAddress = fileDire;
                    }
                    var status = await _employeeBL.Upsert(emp);
                    return Json(status);
                }

                String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage + " " + v.Exception));

                return Json(new BLStatus { Message = messages, IsError = true });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = ex.Message, IsError = true, StatusCode = "500" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EducationCreate(EmployeeEducation edu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (edu.EmpEducationId > 0)
                    {
                        edu.UpdatedBy = _global.GetUserID();
                        edu.UpdatedDate = DateTime.Now;
                    }
                    var status = await _employeeBL.Upsert(edu);

                    return Json(status);
                }

                String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage + " " + v.Exception));

                return Json(new BLStatus { Message = messages, IsError = true });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = ex.Message, IsError = true, StatusCode = "500" });
            }

        }


        [HttpPost]
        public async Task<IActionResult> ExperienceCreate(EmployeeExperience exp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (exp.EmpExpID > 0)
                    {
                        exp.UpdatedBy = _global.GetUserID();
                        exp.UpdatedDate = DateTime.Now;
                    }
                    var status = await _employeeBL.Upsert(exp);
                    return Json(status);
                }

                String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage + " " + v.Exception));

                return Json(new BLStatus { Message = messages, IsError = true });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = ex.Message, IsError = true, StatusCode = "500" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> TrainingCreate(EmployeeTraining training)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = _global.GetUserID();
                    if (training.EmpTrainingID > 0)
                    {
                        training.UpdatedBy = userid;
                        training.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        training.AddedBy = userid;
                        training.AddedDate = DateTime.Now;
                    }
                    var status = await _employeeBL.Upsert(training);
                    return Json(status);
                }

                String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage + " " + v.Exception));

                return Json(new BLStatus { Message = messages, IsError = true });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = ex.Message, IsError = true, StatusCode = "500" });
            }

        }


        [HttpPost]
        public async Task<IActionResult> PaymentDetailsCreate(PayDetails payDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = _global.GetUserID();
                    if (payDetails.PayDetailsId > 0)
                    {
                        payDetails.OrgId = _global.GetOrgId();
                        payDetails.ClientId = _global.GetClientId();
                        payDetails.UpdatedBy = userid;
                        payDetails.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        payDetails.OrgId = _global.GetOrgId();
                        payDetails.ClientId = _global.GetClientId();
                        payDetails.AddedBy = userid;
                        payDetails.AddedDate = DateTime.Now;
                    }
                    var status = await _employeeBL.Upsert(payDetails);
                    return Json(status);
                }

                String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage + " " + v.Exception));

                return Json(new BLStatus { Message = messages, IsError = true });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = ex.Message, IsError = true, StatusCode = "500" });
            }

        }




        [HttpGet]
        [Route("[controller]/Edit/{id:int:min(1)}")]
        public async Task<IActionResult> Edit(int id)
        {
            #region Access
            var roleid = _global.GetRoleID();
            var controller = RouteData.Values["controller"];
            var action = RouteData.Values["action"];
            var url = $"{controller}/{action}";
            ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
            ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
            ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
            ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
            #endregion
            var employee = await _mediator.Send(new GetEmployeeByIdQuery() { EmpId = id });
            await GetDropdownAsync(employee);
            return View("Create", employee);
        }

        [HttpGet]
        //[Route("[controller]/GetEducation/{id:int:min(1)}")]
        public async Task<IActionResult> GetEducation(int eduId)
        {
            var education = await _mediator.Send(new GetEmployeeEducationByIdQuery() { EmpEducationId = eduId });
            return Json(education);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpEducation(int empId)
        {
            var education = await _mediator.Send(new GetEmployeeEducationByEmpIdQuery() { EmpId = empId });
            return Json(education);
        }



        [HttpGet]
        public async Task<IActionResult> GetExperience(int expId)
        {
            var education = await _mediator.Send(new GetEmployeeExperienceByIdQuery() { EmpExperienceId = expId });
            return Json(education);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpExperience(int empId)
        {
            var education = await _mediator.Send(new GetEmployeeExperienceByEmpIdQuery() { EmpId = empId });
            return Json(education);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmpTraining(int empId)
        {
            var training = await _mediator.Send(new GetEmployeeTrainingByEmpIdQuery() { EmpId = empId });
            return Json(training);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                #region Access
                var roleid = _global.GetRoleID();
                var controller = RouteData.Values["controller"];
                var action = RouteData.Values["action"];
                var url = $"{controller}/{action}";
                ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
                ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
                ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
                ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
                #endregion
                string userid = _global.GetUserID();
                var data = await _employeeBL.Delete(id, userid);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpDelete]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            try
            {
                var data = await _mediator.Send(new DeleteEmployeeEducationCommand() { EmpEduId = id });
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = ex.Message, IsError = true, StatusCode = "500" });
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var data = await _mediator.Send(new DeleteEmployeeExperienceCommand() { EexpId = id });
            return Json(data);
        }

        public async Task<IActionResult> GetEmployeeInfoByEmpId(int id)
        {
            var employee = await _mediator.Send(new GetEmpHistoryInfoByEmpIdQuery() { EmpId = id });
            return Json(employee);
        }

        [NonAction]
        private async Task GetDropdownAsync(Employee emp = null)
        {
            var compId = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]));
            var orgid = _global.GetOrgId();
            var clientId = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientId);
            ViewBag.Gender = _dropdown.GenderDropdown(emp?.Gender);
            ViewBag.BloodGroup = _dropdown.BloodGroupDropdown(emp?.EmployeeDetails?.BloodGroup);
            ViewBag.Religion = _dropdown.ReligionDropdown(emp?.EmployeeDetails?.Religion);

            var EmpStatusID = 0;
            var SectId = 0;
            var DeptId = 0;
            var EmpCatId = 0;
            var DesigId = 0;
            if (emp != null)
            {
                if (emp.EmployeeHistory != null)
                {
                    if (emp.EmployeeHistory.EmpStatusID != null)
                    {
                        EmpStatusID = (int)emp.EmployeeHistory.EmpStatusID;
                    }
                    if (emp.EmployeeHistory.SectId != null)
                    {
                        SectId = (int)emp.EmployeeHistory.SectId;
                    }
                    if (emp.EmployeeHistory.DeptId != null)
                    {
                        DeptId = (int)emp.EmployeeHistory.DeptId;
                    }
                    if (emp.EmployeeHistory.EmpCatId != null)
                    {
                        EmpCatId = (int)emp.EmployeeHistory.EmpCatId;
                    }
                    if (emp.EmployeeHistory.DesigId != null)
                    {
                        DesigId = (int)emp.EmployeeHistory.DesigId;
                    }
                }
            }




            ViewBag.EmployeeStatus = await _dropdown.EmployeeStatusDropdown(compId, orgid, EmpStatusID);
            ViewBag.EmployeeType = await _dropdown.EmployeeTypeDropdown(compId, orgid, emp?.EmpTypeId ?? 0);
            ViewBag.Section = await _dropdown.SectionDropdown(compId, orgid, SectId);
            ViewBag.Department = await _dropdown.DepartmentDropdown(compId, orgid, DeptId);
            ViewBag.EmployeeCategory = await _dropdown.EmployeeCategoryDropdown(compId, orgid, clientId, EmpCatId);
            ViewBag.Designation = await _dropdown.DesignationDropdown(compId, orgid, DesigId);
            ViewBag.PayScale = await _dropdown.PayscaleDropdown(compId, orgid, DesigId);

            ViewBag.Country = await _dropdown.CountryDropdown(emp?.EmployeeDetails?.CountryId ?? 0);

            ViewBag.PreDistrict = await _dropdown.DistrictDropdown(emp?.EmployeeDetails?.CountryId ?? 0, emp?.EmployeeDetails?.PresentDistrictId ?? 0);
            ViewBag.PreThana = await _dropdown.ThanaDropdown(0, emp?.EmployeeDetails?.PresentThanaId ?? 0);

            ViewBag.PerDistrict = await _dropdown.DistrictDropdown(emp?.EmployeeDetails?.CountryId ?? 0, emp?.EmployeeDetails?.PermanantDistrictId ?? 0);
            ViewBag.PerThana = await _dropdown.ThanaDropdown(0, emp?.EmployeeDetails?.PermanantThanaId ?? 0);

            ViewBag.Employee = await _dropdown.EmployeeADropdown(clientId, orgid);
            ViewBag.PayGroupId = await _dropdown.PayGroupDropdown(orgid);
            ViewBag.AnnualPayId = await _dropdown.AnnualPayDropdown(orgid);
            ViewBag.PaymentModeId = await _dropdown.PayModeDropdown(orgid);
            ViewBag.PaymentTypeId = await _dropdown.PaymentTypeDropdown(orgid);
            ViewBag.TaxId = await _dropdown.TaxDropdown(orgid);
            ViewBag.BankId = await _dropdown.BankDropdown(orgid);


        }
        public async Task<JsonResult> GetEmployeeByCardNo(string CardNo)
        {
            var empdetails = await _global.GetEmployeeByCardNo(CardNo);
            return Json(empdetails);
        }

        public async Task<IActionResult> ContractAgreement()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContractAgreementLoadData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var search = Request.Form["search[value]"].FirstOrDefault();
                var EmpCategoryId_Filter = Convert.ToInt32(Request.Form["EmpCategoryId_Filter"].FirstOrDefault() ?? "0");
                var SectionId_Filter = Convert.ToInt32(Request.Form["SectionId_Filter"].FirstOrDefault() ?? "0");
                var DepartmentId_Filter = Convert.ToInt32(Request.Form["DepartmentId_Filter"].FirstOrDefault() ?? "0");
                var DesignationId_Filter = Convert.ToInt32(Request.Form["DesignationId_Filter"].FirstOrDefault() ?? "0");
                var CardNo_Filter = Request.Form["CardNo_Filter"].FirstOrDefault();
                var EmpName_Filter = Request.Form["EmpName_Filter"].FirstOrDefault();
                var OrgId = _global.GetOrgId();
                var totalrecord = 0;
                var UserId = _global.GetUserID(); ;
                //var Role = DataEncryption.DecryptString(Request.Cookies["Role"]);
                var ComId = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var employees = new List<EmployeeVM>();
                EmployeeFilterVM filterVM = new EmployeeFilterVM()
                {
                    EmpCategoryId_Filter = EmpCategoryId_Filter,
                    SectionId_Filter = SectionId_Filter,
                    DepartmentId_Filter = DepartmentId_Filter,
                    DesignationId_Filter = DesignationId_Filter,
                    CardNo_Filter = CardNo_Filter,
                    EmpName_Filter = EmpName_Filter,
                    OrgId = OrgId
                };

                employees = await _mediator.Send(new GetContractAgreementListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = ComId, ClientId = ClientId, EmployeeFilterVM = filterVM });


                if (employees.Count != 0)
                {
                    if (employees.FirstOrDefault().TotalCount != 0)
                    {
                        totalrecord = employees.FirstOrDefault().TotalCount;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = employees });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> ContractReport(int id)
        {

            EmployeeReport employeeReport = new EmployeeReport();

            var orgid = _global.GetOrgId();
            var organaisation = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = orgid });
            var employee = await _mediator.Send(new GetEmployeeByIdQuery() { EmpId = id });
            // var employeeDoc = await _mediator.Send(new GetAllEmployeeNewDocByEmpIDQuery() { EmpId = id });

            employeeReport.Employee = employee;
            employeeReport.OrganisationVM = organaisation;
            //employeeReport.EmpNewDocuments = employeeDoc;

            var demoViewLandscape = new ViewAsPdf("ContractReport", employeeReport)
            {
                FileName = "ContractReport.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };

            return demoViewLandscape;
            //return new ViewAsPdf("EmployeeReport", employeeReport);
        }

        public async Task<IActionResult> AllEmployeeReport()
        {
            EmployeeReport employeeReport = new EmployeeReport();

            var orgid = _global.GetOrgId();
            var employees = new List<EmployeeVM>();

            var draw = "1";
            var start = "0";
            var length = "1000";
            var ordercolumn = "0";
            var orderdirection = "desc";
            var search = "";
            var ComId = _global.GetCompID();
            var ClientId = _global.GetClientId();
            EmployeeFilterVM filterVM = new EmployeeFilterVM()
            {
                EmpCategoryId_Filter = 0,
                SectionId_Filter = 0,
                DepartmentId_Filter = 0,
                DesignationId_Filter = 0,
                CardNo_Filter = "",
                EmpName_Filter = "",
                OrgId = orgid
            };



            var organaisation = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = orgid });
            employees = await _mediator.Send(new GetEmployeeListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = ComId, ClientId = ClientId, EmployeeFilterVM = filterVM });

            employeeReport.AllEmployee = employees;
            employeeReport.OrganisationVM = organaisation;

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var EmpID = Convert.ToInt32(employee.EmpId);

                    EmpDocumentsVM Visadata = await _mediator.Send(new GetAllEmployeeNewDocByTypeQuery { EmpID = EmpID, DocType = "Visa" });
                    EmpDocumentsVM Passportdata = await _mediator.Send(new GetAllEmployeeNewDocByTypeQuery { EmpID = EmpID, DocType = "Passport" });
                    EmpDocumentsVM EUSSdata = await _mediator.Send(new GetAllEmployeeNewDocByTypeQuery { EmpID = EmpID, DocType = "Euss_Time_limit" });
                    EmpDocumentsVM DBSdata = await _mediator.Send(new GetAllEmployeeNewDocByTypeQuery { EmpID = EmpID, DocType = "DBS" });
                    EmpDocumentsVM NIDdata = await _mediator.Send(new GetAllEmployeeNewDocByTypeQuery { EmpID = EmpID, DocType = "NationalID" });


                    var expiryDt = "";
                    var PostCode = "";
                    var DBSType = "";
                    var DBSRemarks = "";
                    var EUSSPostCode = "";
                    var EUSSRemarks = "";

                    if (Visadata != null)
                    {
                        if (Visadata.ExpiryDate != null)
                        {
                            expiryDt = Visadata.ExpiryDate.ToString();
                        }
                    }

                    if (Passportdata != null)
                    {
                        if (!string.IsNullOrEmpty(Passportdata.PostCode.ToString()))
                        {
                            PostCode = Passportdata.PostCode.ToString();
                        }
                    }

                    if (EUSSdata != null)
                    {
                        if (!string.IsNullOrEmpty(EUSSdata.PostCode.ToString()))
                        {
                            EUSSPostCode = EUSSdata.PostCode.ToString();
                        }

                        if (!string.IsNullOrEmpty(EUSSdata.Remarks.ToString()))
                        {
                            EUSSRemarks = EUSSdata.Remarks.ToString();
                        }
                    }

                    if (NIDdata != null) { }

                    if (DBSdata != null)
                    {
                        if (!string.IsNullOrEmpty(DBSdata.PostCode.ToString()))
                        {
                            DBSType = DBSdata.DBSType.ToString();
                        }

                        if (!string.IsNullOrEmpty(DBSdata.Remarks.ToString()))
                        {
                            DBSRemarks = DBSdata.Remarks.ToString();
                        }
                    }




                    employeeReport.AllEmployee.FirstOrDefault().VisaExpiryDate = expiryDt;
                    employeeReport.AllEmployee.FirstOrDefault().PassportNo = PostCode.ToString();
                    employeeReport.AllEmployee.FirstOrDefault().DBSDetails = DBSType + " " + DBSRemarks.ToString();
                    employeeReport.AllEmployee.FirstOrDefault().EUSSDetails = EUSSPostCode.ToString() + " " + EUSSRemarks.ToString();
                }
            }

            var demoViewLandscape = new ViewAsPdf("AllEmployeeReport", employeeReport)
            {
                FileName = "AllEmployeeReport.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };

            return demoViewLandscape;
        }

        public async Task<IActionResult> EmployeeReport(int id)
        {
            EmployeeReport employeeReport = new EmployeeReport();

            var orgid = _global.GetOrgId();
            var organaisation = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = orgid });
            var employee = await _mediator.Send(new GetEmployeeByIdQuery() { EmpId = id });
            var employeeDoc = await _mediator.Send(new GetAllEmployeeNewDocByEmpIDQuery() { EmpId = id });

            employeeReport.Employee = employee;
            employeeReport.OrganisationVM = organaisation;
            employeeReport.EmpNewDocuments = employeeDoc;

            var demoViewLandscape = new ViewAsPdf("EmployeeReport", employeeReport)
            {
                FileName = "EmployeeReport.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };

            return demoViewLandscape;
            //return new ViewAsPdf("EmployeeReport", employeeReport);
        }

    }
}
