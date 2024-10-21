using Application.Tasks.Queries.QAdvanceSanction;
using Application.Tasks.Queries.QEmployeeShift;
using AutoMapper;
using UKHRM.Helpers;
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

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class AdvanceSanctionController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IAdvanceSanctionBL _advanceSanctionBL;
        //private readonly ILogger _logger;

        public AdvanceSanctionController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IAdvanceSanctionBL EmployeeShiftBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _advanceSanctionBL = EmployeeShiftBL;
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
            //ViewBag.EmpCategory = await _dropdown.EmployeeCategoryDropdown(compId);
            ViewBag.Designation = await _dropdown.DesignationDropdown(compId, OrgId);
            ViewBag.Section = await _dropdown.SectionDropdown(compId,OrgId);
            ViewBag.Rules = await _dropdown.RulesDropdown("Calculation Rule");

            ViewBag.Department = await _dropdown.DepartmentDropdown(compId, OrgId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(AdvanceSanctionFilter filter)
        {
            try
            {
                filter.OrgId = _global.GetOrgId();
                var data = await _mediator.Send(new GetEmpByAdvanceSanctionQuery { AdvanceSanctionFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployeeSanction(AdvanceSanctionFilter filter)
        {
            try
            {
                filter.OrgId = _global.GetOrgId();
                var data = await _mediator.Send(new GetAdvanceSanctionListQuery { AdvanceSanctionFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<Advance> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.AdvanceID > 0)
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

                    var status = await _advanceSanctionBL.Upsert(model);
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
        public async Task<IActionResult> CalculateInstallment(Advance model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _advanceSanctionBL.CheckInstallment(model);
                    return Json(new BLStatus { Data = result, });
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
        public async Task<IActionResult> Delete(int advanceId)
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
                var status = await _advanceSanctionBL.Delete(advanceId);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }



        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByAdvanceId(int advanceId)
        {
            try
            {
                if (advanceId < 1) return BadRequest();

                var data = await _mediator.Send(new GetAdvanceSanctionByIdQuery { AdvanceId = advanceId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }


    }
}
