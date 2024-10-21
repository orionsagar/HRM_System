using Application.Tasks.Queries.QEmployeeShift;
using AutoMapper;
using UKHRM.Helpers;
using Business.Schedule;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class EmployeeShiftController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployeeShiftBL _employeeShiftBL;
        //private readonly ILogger _logger;

        public EmployeeShiftController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IEmployeeShiftBL EmployeeShiftBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _employeeShiftBL = EmployeeShiftBL;
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
            ViewBag.Schedule = await _dropdown.ScheduleDropdown(OrgId);
            //ViewBag.Shift = await _dropdown.ShiftDropdown(compId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(EmpShiftFilter filter)
        {
            try
            {
                filter.OrgId = _global.GetOrgId();
                var data = await _mediator.Send(new GetEmpByShiftQuery { EmpShiftFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<EmployeeShift> model)
        {
            try
            {
                var OrgId = _global.GetOrgId();
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        if (item.EmpShiftId > 0)
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
                        item.OrgId = OrgId;
                    }

                    var status = await _employeeShiftBL.Upsert(model);
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
        public async Task<IActionResult> DeleteByEmpId(List<int> empIds)
        {
            try
            {
                var status = await _employeeShiftBL.DeleteByEmpId(empIds);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int empShiftid)
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
                var status = await _employeeShiftBL.Delete(empShiftid);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }



        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByEmpId(int empId)
        {
            try
            {
                if (empId < 1) return BadRequest();

                var data = await _mediator.Send(new GetEmpShiftByEmpIdQuery { EmpId = empId });
                return Json(new BLStatus { Data = data.OrderByDescending(a=>a.EffectiveFrom) });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }


    }
}
