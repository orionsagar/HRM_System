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
using UKHRM.Helpers;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class OtherAllowanceController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOtherAllowanceBL _otherAllowanceBL;
        //private readonly ILogger _logger;

        public OtherAllowanceController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IOtherAllowanceBL otherAllowanceBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _otherAllowanceBL = otherAllowanceBL;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
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
            var OrgId = _global.GetOrgId();
           // ViewBag.EmpCategory = await _dropdown.EmployeeCategoryDropdown(compId);
            ViewBag.Designation = await _dropdown.DesignationDropdown(compId, OrgId);
            ViewBag.Section = await _dropdown.SectionDropdown(compId, OrgId);
            ViewBag.Rules = await _dropdown.RulesDropdown("Calculation Rule");
             ViewBag.Department = await _dropdown.DepartmentDropdown(compId, OrgId);
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(OrgId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(OtherAllowanceFilter filter)
        {
            try
            {
                filter.OrgId = _global.GetOrgId();
                var data = await _mediator.Send(new GetEmpByOtherAllowanceQuery { OtherAllowanceFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeOtherAllowance(OtherAllowanceFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetOtherAllowanceListQuery { OtherAllowanceFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<OtherAllowance> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.OtherAllowanceID > 0)
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

                    var status = await _otherAllowanceBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteByEmpId(List<int> empIds)
        //{
        //    try
        //    {
        //        var status = await _advanceSanctionBL.DeleteByEmpId(empIds);
        //        return Json(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
        //    }
        //}

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int otherAllowanceId)
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
                var status = await _otherAllowanceBL.Delete(otherAllowanceId);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }



        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByOtherAllowanceId(int otherAllowanceId)
        {
            try
            {
                if (otherAllowanceId < 1) return BadRequest();

                var data = await _mediator.Send(new GetOtherAllowanceByIdQuery { OtherAllowanceId = otherAllowanceId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }


    }
}
