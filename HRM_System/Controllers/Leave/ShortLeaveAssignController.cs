using Application.Tasks.Queries.QShortLeaveAssign;
using AutoMapper;
using UKHRM.Helpers;
using Business.Leave;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace UKHRM.Controllers.BonusNAllowance
{
    [Authorize]
    public class ShortLeaveAssignController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IShortLeaveAssignBL _shortLeaveAssignBL;
        //private readonly ILogger _logger;

        public ShortLeaveAssignController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IShortLeaveAssignBL shortLeaveAssignBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _shortLeaveAssignBL = shortLeaveAssignBL;
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
            await DropdownAsync();
            return View();
        }

        [NonAction]
        private async Task DropdownAsync()
        {
            var compId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            //ViewBag.EmpCategory = await _dropdown.EmployeeCategoryDropdown(compId);
            ViewBag.Designation = await _dropdown.DesignationDropdown(compId, OrgId);
            ViewBag.Section = await _dropdown.SectionDropdown(compId,OrgId);
            ViewBag.Department = await _dropdown.DepartmentDropdown(compId, OrgId);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(ShortLeaveAssignFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetEmployeeListForShortLeaveAssignQuery { ShortLeaveAssignFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadShortLeaveAssign(ShortLeaveAssignFilter filter)
        {
            try
            {
                var data = await _mediator.Send(new GetShortLeaveAssignListQuery { ShortLeaveAssignFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<ShortLeaveAssign> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.ShortLeaveAssignId > 0)
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

                    var status = await _shortLeaveAssignBL.Upsert(model);
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
        public async Task<IActionResult> Delete(int shortLeaveAssignId)
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
                var status = await _shortLeaveAssignBL.Delete(shortLeaveAssignId);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }



        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetById(int shortLeaveAssignId)
        {
            try
            {
                if (shortLeaveAssignId < 1) return BadRequest();

                var data = await _mediator.Send(new GetShortLeaveAssignByIdQuery { ShortLeaveAssignId = shortLeaveAssignId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }




    }
}
