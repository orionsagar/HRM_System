using Application.Tasks.Queries.QOtherAllowance;
using Application.Tasks.Queries.QEmployeeShift;
using AutoMapper;
using Business.HR;
using Business.Schedule;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Business.BonusNAllowance;
using Application.Tasks.Queries.QAttBonus;
using Application.Tasks.Queries.QEmployee;
using Application.Tasks.Queries.QFestivalBonus;
using Application.Tasks.Queries.QPerformanceBonus;
using UKHRM.Helpers;
using Application.Tasks.Queries.QCompany;
using System.Globalization;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class BonusController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IAttendanceBonusBL _attBonusBL;
        private readonly IFestivalBonusBL _festivalBonusBL;
        private readonly IPerformanceBonusBL _performanceBonusBL;
        //private readonly ILogger _logger;

        public BonusController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IAttendanceBonusBL attBonusBL, IMapper mapper, IFestivalBonusBL festivalBonusBL, IPerformanceBonusBL performanceBonusBL)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _attBonusBL = attBonusBL;
            _mapper = mapper;
            _festivalBonusBL = festivalBonusBL;
            _performanceBonusBL = performanceBonusBL;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(EmployeeFilterVM filter)
        {
            try
            {
                filter.OrgId = _global.GetOrgId();
                var data = await _mediator.Send(new GetEmployeeImprovedListQuery { EmployeeFilterVM = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeNoInBonusSet(EmployeeFilterVM filter)
        {
            try
            {
                filter.OrgId = _global.GetOrgId();
                var data = await _mediator.Send(new GetEmployeeNotINBOnusSetListQuery { EmployeeFilterVM = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private async Task DropdownAsync()
        {
            var compId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            //ViewBag.EmpCategory = await _dropdown.EmployeeCategoryDropdown(compId);
            ViewBag.Designation = await _dropdown.DesignationDropdown(compId, OrgId);
            ViewBag.Section = await _dropdown.SectionDropdown(compId,OrgId);
            ViewBag.Rules = await _dropdown.RulesDropdown("Calculation Rule");
            ViewBag.Festival = await _dropdown.FestivalTypeDropdown();
            ViewBag.Department = await _dropdown.DepartmentDropdown(compId, OrgId);
        }


        #region Attendance Bonus 
        /// <summary>
        /// End of attendance bonus
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Attendance()
        {
            await DropdownAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeAttBonus(BonusFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetEmpByAttBonusQuery { AttBonusFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttendanceBonusCreate(List<AttendanceBonus> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.AttendanceBonusID > 0)
                        {
                            item.UpdatedBy = _global.GetUserID();
                            item.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            item.AddedBy = _global.GetUserID();
                            item.AddedDate = DateTime.Now;
                        }
                        item.CompId = _global.GetCompID();
                    }

                    var status = await _attBonusBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttBonus(int attendanceBonusID)
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
                var status = await _attBonusBL.Delete(attendanceBonusID);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }


        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByAttBonusId(int attBonusId)
        {
            try
            {
                if (attBonusId < 1) return BadRequest();

                var data = await _mediator.Send(new GetAttBonusByIdQuery { AttBonusId = attBonusId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }

        #endregion


        #region Festival Bonus 

        public async Task<IActionResult> Festival()
        {
            await DropdownAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeByFestivalType(int festivalTypeId,int bonusId)
        {
          
            try
            {
                if (bonusId < 1 && festivalTypeId<1) return BadRequest();

                var data = await _mediator.Send(new GetEmployeeByFestivalTypeQuery { FestivalId = festivalTypeId, BonusId = bonusId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }


        }

        public async Task<IActionResult> FestivalBonusSetup()
        {
            await DropdownAsync();
            return View();
        }

        public async Task<IActionResult> FestivalBonusGenerate()
        {
            await DropdownAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFestivalBonusSetup(List<FestivalBonusSetup> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.BonusId > 0)
                        {
                            item.UpdatedBy = _global.GetUserID();
                            item.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            item.AddedBy = _global.GetUserID();
                            item.AddedDate = DateTime.Now;
                        }
                        item.CompId = _global.GetCompID();
                    }

                    var status = await _festivalBonusBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }
       

        [HttpPost]
        public async Task<IActionResult> CreateFestivalType(FestivalType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                

                    var status = await _festivalBonusBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FestivalBonusCreate(List<FestivalBonus> model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    foreach (var item in model)
                    {
                        if (item.FestivalBonusID > 0)
                        {
                            item.UpdatedBy = _global.GetUserID();
                            item.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            item.AddedBy = _global.GetUserID();
                            item.AddedDate = DateTime.Now;
                        }
                        item.CompId = _global.GetCompID();
                    }

                    var status = await _festivalBonusBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeFestivalBonus(BonusFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetEmpByFestivalBonusQuery { FestivalBonusFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeSearchFestivalBonusSetUp(BonusFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetEmployeeSearchFestivalBonusSetUpQuery { FestivalBonusFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFestivalBonus(int festivalBonusID)
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
                var status = await _festivalBonusBL.Delete(festivalBonusID);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFestivalBonusSetUp(int BonusID)
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
                var status = await _festivalBonusBL.DeleteBonusSetUp(BonusID);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }




        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByFestivalBonusId(int festivalBonusId)
        {
            try
            {
                if (festivalBonusId < 1) return BadRequest();

                var data = await _mediator.Send(new GetFestivalBonusByIdQuery { FestivalBonusId = festivalBonusId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }



        [HttpGet]
        public async Task<IActionResult> FestivalType()
        {
            var ComId = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]));

            var companys = await _mediator.Send(new GetCompanyByIdQuery { CompID = ComId });
            ViewBag.Comapny = _mapper.Map<CompanyViewModel>(companys);
            await DropdownAsync();
            return PartialView("_FestivalType");
        }


        #endregion


        #region Performance Bonus 

        public async Task<IActionResult> Performance()
        {
            await DropdownAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerformanceBonusCreate(List<PerformanceBonus> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.PerformanceBonusID > 0)
                        {
                            item.UpdatedBy = _global.GetUserID();
                            item.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            item.AddedBy = _global.GetUserID();
                            item.AddedDate = DateTime.Now;
                        }
                        item.CompId = _global.GetCompID();
                    }

                    var status = await _performanceBonusBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeePerformanceBonus(BonusFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetEmpByPerformanceBonusQuery { PerformanceBonusFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePerformanceBonus(int performanceBonusID)
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
                var status = await _performanceBonusBL.Delete(performanceBonusID);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }


        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByPerformanceBonusId(int performanceBonusID)
        {
            try
            {
                if (performanceBonusID < 1) return BadRequest();

                var data = await _mediator.Send(new GetPerformanceBonusByIdQuery { PerformanceBonusId = performanceBonusID });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }

        #endregion



    }
}
