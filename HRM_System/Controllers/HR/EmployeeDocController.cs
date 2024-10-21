using Application.Tasks.Queries.QEmployee;
using Business.HR;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using UKHRM.Helper;
using UKHRM.Helpers;
using System.IO;
using Microsoft.AspNetCore.Http;
using Application.Tasks.Commands.COrganisation;
using Microsoft.CodeAnalysis.Differencing;
using Persistence.Repository.Organisation;
using Application.Tasks.Queries.QOrganisation;
using Application.Tasks.Queries.QUserInfo;
using Application.Tasks.Commands.CEmployeeDocuments;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Application.Tasks.Queries.QEmployeeDocument;
using Application.Tasks.Queries.QEmployeeType;
using Domains.Models;
using Application.Tasks.Commands.CLevel;
using Application.Tasks.Commands.CTransactionLog;
using Utility;
using Microsoft.AspNetCore.Authorization;
using Persistence.DAL;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class EmployeeDocController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IEmployeeBL _employeeBL;
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly CommonDropdown _common;

        public EmployeeDocController(CommonDropdown dropdown = null, IEmployeeBL employeeBL = null, IMediator mediator = null, IGlobalHelper global = null, IWebHostEnvironment webHostEnvironment = null, CommonDropdown common = null)
        {
            _dropdown = dropdown;
            _employeeBL = employeeBL;
            _mediator = mediator;
            _global = global;
            this.webHostEnvironment = webHostEnvironment;
            _common = common;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                var empDocLists = await _mediator.Send(new SP_Dt_EmpDocListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ClientId = ClientId, OrgId = orgid });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
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
            var ClientId = _global.GetClientId();
            var CompID = _global.GetCompID();
            var orgid = _global.GetOrgId();

            ViewBag.Employee = await _common.EmployeeADropdown(ClientId, orgid);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDocumentsVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //
                    if (model.UploadDocumentFile != null)
                    {
                        model.UploadDocument = UploadedFile(model.UploadDocumentFile);
                    }

                    // 
                    if (model.VisaUploadDocumentFile != null)
                    {
                        model.VisaUploadDocument = UploadedFile(model.VisaUploadDocumentFile);
                    }

                    //
                    if (model.EussUploadDocumentFile != null)
                    {
                        model.EussUploadDocument = UploadedFile(model.EussUploadDocumentFile);
                    }

                    //
                    if (model.DBSUploadDocumentFile != null)
                    {
                        model.DBSUploadDocument = UploadedFile(model.DBSUploadDocumentFile);
                    }

                    //
                    if (model.IdUploadDocumentFile != null)
                    {
                        model.IdUploadDocument = UploadedFile(model.IdUploadDocumentFile);
                    }

                    //
                    if (model.OtherUploadDocumentFile != null)
                    {
                        model.OtherUploadDocument = UploadedFile(model.OtherUploadDocumentFile);
                    }



                    if (model.EmpDocID == 0)
                    {
                        model.AddedBy = _global.GetUserID();
                        model.AddedDate = DateTime.Now;
                        var orgid = await _mediator.Send(new CreateEmployeeDocumentCommand { EmployeeDocuments = model });
                    }
                    else
                    {
                        model.UpdatedBy = _global.GetUserID();
                        model.UpdatedDate = DateTime.Now;
                        var orgid = await _mediator.Send(new EditEmployeeDocumentCommand { EmployeeDocuments = model });
                    }


                    //return Json(new BLStatus { Data = orgid, IsError = false, Message = "Data Saved Successfully", StatusCode= "200"});
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                    var ComId = _global.GetCompID();
                    var OrgId = _global.GetOrgId();
                    ViewBag.Designation = await _dropdown.DesignationDropdown(ComId, OrgId);
                    ViewBag.Client = await _dropdown.ClientDropdown();
                    ViewBag.Country = await _dropdown.CountryDropdown();
                    if (model.EmpDocID > 0)
                    {
                        return RedirectToAction(nameof(Edit), model.EmpDocID);
                    }
                    return View(model);
                    //return Json(new BLStatus { IsError = true, Message = $"Unable To Saved. error is {ModelState.Values}", StatusCode = "500" });
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
                //return Json(new BLStatus { IsError = true, Message = $"Unable To Saved. error is {ex.Message}", StatusCode = "500" });
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Action = "Edit";
            var ClientId = _global.GetClientId();
            if (id == null)
            {
                return Redirect("/error/404");
            }
            var dbdata = new EmployeeDocumentsVM();
            dbdata = await _mediator.Send(new GetAllEmployeeDocQuery { EmpDocId = Convert.ToInt32(id) });
            var user = await _mediator.Send(new GetUserBy_OrgId_RoleId_ClientId_Query { OrgId = Convert.ToInt32(id) });
            //if (user != null)
            //{
            //    orgdata.SA_FirstName = user.FirstName;
            //    orgdata.SA_LastName = user.LastName;
            //    orgdata.SA_Email = user.Email;
            //}

            var orgid = _global.GetOrgId();
            if (dbdata == null)
            {
                return Redirect("/error/404");
            }
            ViewBag.Employee = await _common.EmployeeADropdown(ClientId, orgid);
            var employee = await _mediator.Send(new GetEmpHistoryInfoByEmpIdQuery() { EmpId = dbdata.EmpID });
            dbdata.Name = employee.Name;
            dbdata.CardNo = employee.CardNo.ToString();
            dbdata.Designation = employee.DesignationName;
            dbdata.Address = employee.Address;

            return View("Create", dbdata);
        }

        private string UploadedFile(IFormFile model)
        {
            string uniqueFileName = null;

            if (model != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "userfiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return "userfiles/" + uniqueFileName;
        }


        [HttpGet]
        public async Task<IActionResult> DocEntry()
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
            var ClientId = _global.GetClientId();
            var CompID = _global.GetCompID();
            var orgid = _global.GetOrgId();

            ViewBag.Employee = await _common.EmployeeADropdown(ClientId, orgid);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DocEntry(EmpDocumentsVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ClientId = _global.GetClientId();
                    var CompID = _global.GetCompID();
                    var orgid = _global.GetOrgId();

                    List<DocumentLists> files = new List<DocumentLists>();
                    if (model.Files != null)
                    {
                        if (model.Files.Count > 0)
                        {

                            foreach (var file in model.Files)
                            {
                                var fn = UploadedFile(file);

                                files.Add(new DocumentLists
                                {
                                    DocFiles = fn,
                                    DocName = file.Name,
                                    DocSize = "0",
                                });
                            }
                        }
                    }

                    model.OrgId = orgid;
                    model.ClientId = ClientId;

                    model.AllFiles = files;

                    if (model.EmpNewDocumentsID == 0)
                    {
                        model.AddedBy = _global.GetUserID();
                        model.AddedDate = DateTime.Now;
                        var ins = await _mediator.Send(new CreateEmpNewDocumentCommand { EmployeeDocuments = model });
                    }
                    else
                    {
                        model.UpdatedBy = _global.GetUserID();
                        model.UpdatedDate = DateTime.Now;
                        var upd = await _mediator.Send(new EditEmpNewDocumentCommand { EmployeeDocuments = model });
                    }


                    //return Json(new BLStatus { Data = orgid, IsError = false, Message = "Data Saved Successfully", StatusCode= "200"});
                    return RedirectToAction(nameof(DocIndex));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");

                    var ClientId = _global.GetClientId();
                    var CompID = _global.GetCompID();
                    var orgid = _global.GetOrgId();

                    ViewBag.Employee = await _common.EmployeeADropdown(ClientId, orgid);
                    if (model.EmpNewDocumentsID > 0)
                    {
                        return RedirectToAction(nameof(Edit), model.EmpNewDocumentsID);
                    }
                    return View(model);
                    //return Json(new BLStatus { IsError = true, Message = $"Unable To Saved. error is {ModelState.Values}", StatusCode = "500" });
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message.ToString();
            }


            return View(model);
        }


        public async Task<IActionResult> DocIndex()
        {
            var ClientId = _global.GetClientId();
            var CompID = _global.GetCompID();
            var orgid = _global.GetOrgId();

            ViewBag.Employee = await _common.EmployeeADropdown(ClientId, orgid);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DocLoadData()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                var empDocLists = await _mediator.Send(new SP_Dt_EmpNewDocListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ClientId = ClientId, OrgId = orgid });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DT_Doc_Notification_Data()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                DataTableParamVM dt = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = orgid,
                    DocType = "Visa"
                };
                var empDocLists = await _mediator.Send(new DT_Doc_Notification_DataQuery() { Param = dt });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().totalcount;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DT_Passport_Notification_Data()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                DataTableParamVM dt = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = orgid,
                    DocType = "Passport"
                };
                var empDocLists = await _mediator.Send(new DT_Doc_Notification_DataQuery() { Param = dt });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().totalcount;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> DT_DBS_Notification_Data()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                DataTableParamVM dt = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = orgid,
                    DocType = "DBS"
                };
                var empDocLists = await _mediator.Send(new DT_Doc_Notification_DataQuery() { Param = dt });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().totalcount;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> DT_EUSS_Notification_Data()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                DataTableParamVM dt = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = orgid,
                    DocType = "Euss_Time_limit"
                };
                var empDocLists = await _mediator.Send(new DT_Doc_Notification_DataQuery() { Param = dt });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().totalcount;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> DT_RWC_Notification_Data()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                DataTableParamVM dt = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = orgid,
                    DocType = "RWC"
                };
                var empDocLists = await _mediator.Send(new DT_Doc_Notification_DataQuery() { Param = dt });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().totalcount;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DT_Others_Notification_Data()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                DataTableParamVM dt = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = orgid,
                    DocType = "Others"
                };
                var empDocLists = await _mediator.Send(new DT_Doc_Notification_DataQuery() { Param = dt });

                //total number of rows counts   
                if (empDocLists.Count > 0)
                {
                    recordsTotal = empDocLists.FirstOrDefault().totalcount;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empDocLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
            }
        }


        public async Task<IActionResult> doc_firstletter(int? id)
        {
            DocLetterViewModel docLetterViewModel = new DocLetterViewModel();
            var dbdata = new EmpDocumentsVM();

            var orgid = _global.GetOrgId();
            var organaisation = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = orgid });


            dbdata = await _mediator.Send(new GetAllEmployeeNewDocQuery { EmpDocId = Convert.ToInt32(id) });

            var empData = await _mediator.Send(new GetEmployeeByEmpIdQuery { EmpId = dbdata.EmpID });


            docLetterViewModel.EmployeeName = empData?.Name;
            docLetterViewModel.DocDate = Convert.ToDateTime(dbdata.IssuedDate).ToString("dd/MM/yyyy");
            docLetterViewModel.LetterDays = "90";
            docLetterViewModel.EmpAddress = empData?.Address;
            docLetterViewModel.DocExpiryDate = Convert.ToDateTime(dbdata.ExpiryDate).ToString("dd/MM/yyyy");
            docLetterViewModel.DocType = dbdata.DocumentType;

            if (Request.Cookies["FullName"] != null)
            {
                docLetterViewModel.SendedBy = Helper.DataEncryption.DecryptString(Request.Cookies["FullName"]).ToString();
            }
            else
            {
                docLetterViewModel.SendedBy = "";
            }

            docLetterViewModel.SenderDesignation = "Owner/Supervisor";
            docLetterViewModel.OrgLogo = StaticData.HostPath + organaisation.LogoPath;


            return View(docLetterViewModel);
        }

        public async Task<IActionResult> doc_secondletter(int? id)
        {
            DocLetterViewModel docLetterViewModel = new DocLetterViewModel();
            var dbdata = new EmpDocumentsVM();

            var orgid = _global.GetOrgId();
            var organaisation = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = orgid });


            dbdata = await _mediator.Send(new GetAllEmployeeNewDocQuery { EmpDocId = Convert.ToInt32(id) });

            var empData = await _mediator.Send(new GetEmployeeByIdQuery { EmpId = dbdata.EmpID });


            docLetterViewModel.EmployeeName = empData.Name;
            docLetterViewModel.DocDate = Convert.ToDateTime(dbdata.IssuedDate).ToString("dd/MM/yyyy");
            docLetterViewModel.LetterDays = "60";
            docLetterViewModel.EmpAddress = empData.Address;
            docLetterViewModel.DocExpiryDate = Convert.ToDateTime(dbdata.ExpiryDate).ToString("dd/MM/yyyy");

            if (Request.Cookies["FullName"] != null)
            {
                docLetterViewModel.SendedBy = Request.Cookies["FullName"];
            }
            else
            {
                docLetterViewModel.SendedBy = "";
            }

            docLetterViewModel.SenderDesignation = "Owner/Supervisor";
            docLetterViewModel.OrgLogo = StaticData.HostPath + organaisation.LogoPath;


            return View(docLetterViewModel);
        }

        public async Task<IActionResult> doc_thirdletter(int? id)
        {
            DocLetterViewModel docLetterViewModel = new DocLetterViewModel();
            var dbdata = new EmpDocumentsVM();

            var orgid = _global.GetOrgId();
            var organaisation = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = orgid });


            dbdata = await _mediator.Send(new GetAllEmployeeNewDocQuery { EmpDocId = Convert.ToInt32(id) });

            var empData = await _mediator.Send(new GetEmployeeByIdQuery { EmpId = dbdata.EmpID });


            docLetterViewModel.EmployeeName = empData.Name;
            docLetterViewModel.DocDate = Convert.ToDateTime(dbdata.IssuedDate).ToString("dd/MM/yyyy");
            docLetterViewModel.LetterDays = "30";
            docLetterViewModel.EmpAddress = empData.Address;
            docLetterViewModel.DocExpiryDate = Convert.ToDateTime(dbdata.ExpiryDate).ToString("dd/MM/yyyy");

            if (Request.Cookies["FullName"] != null)
            {
                docLetterViewModel.SendedBy = Request.Cookies["FullName"];
            }
            else
            {
                docLetterViewModel.SendedBy = "";
            }

            docLetterViewModel.SenderDesignation = "Owner/Supervisor";
            docLetterViewModel.OrgLogo = StaticData.HostPath + organaisation.LogoPath;


            return View(docLetterViewModel);
        }

        public async Task<IActionResult> DocEdit(int? id)
        {
            ViewBag.Action = "Edit";
            var ClientId = _global.GetClientId();

            if (id == null)
            {
                return Redirect("/error/404");
            }
            var dbdata = new EmpDocumentsVM();
            dbdata = await _mediator.Send(new GetAllEmployeeNewDocQuery { EmpDocId = Convert.ToInt32(id) });
            var user = await _mediator.Send(new GetUserBy_OrgId_RoleId_ClientId_Query { OrgId = Convert.ToInt32(id) });


            ViewBag.OtherDocList = await _mediator.Send(new GetEmpAllDocsQuery { EmpId = dbdata.EmpID, EmpDocId = Convert.ToInt32(id) });

            //if (user != null)
            //{
            //    orgdata.SA_FirstName = user.FirstName;
            //    orgdata.SA_LastName = user.LastName;
            //    orgdata.SA_Email = user.Email;
            //}

            var orgid = _global.GetOrgId();
            if (dbdata == null)
            {
                return Redirect("/error/404");
            }

            ViewBag.Employee = await _common.EmployeeADropdown(ClientId, orgid);
            var employee = await _mediator.Send(new GetEmpHistoryInfoByEmpIdQuery() { EmpId = dbdata.EmpID });
            //dbdata.Name = employee.Name;
            dbdata.CardNo = employee.CardNo.ToString();
            dbdata.Designation = employee.DesignationName;
            dbdata.AllFiles = await _global.EmpDocumentList(dbdata.EmpNewDocumentsID);
            //dbdata.Address = employee.Address;

            return View("DocEntry", dbdata);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
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
                var tableData = await _mediator.Send(new GetAllEmployeeNewDocQuery { EmpDocId = Convert.ToInt32(Id) });
                if (tableData != null)
                {
                    await _mediator.Send(new DeleteEmpNewDocumentCommand() { EmpDocId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} Level", DocumentReferance = Id.ToString() });
                }
                return Json(new BLStatus { Message = "Delete Successfully", Data = tableData });
            }
            catch (Exception)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable To Delete" });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteDoc(string Id)
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
                var alldoc = _global.EmpDocumentList(Convert.ToInt32(Id));
                if (alldoc != null)
                {
                    await _global.EmpDocumentDelete(Convert.ToInt32(Id));
                }

                return Json(new BLStatus { Message = "Delete Successfully", Data = alldoc });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable To Delete" });
            }
        }
    }
}
