using Application.Tasks.Commands.CShortLeaveSetup;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QEmployee;
using Application.Tasks.Queries.QShortLeaveSetup;
using UKHRM.Helpers;
using Business.Leave;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers.Leave
{
    public class ShortLeaveSetupController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;
        private readonly ILeaveTransactionBL _leave;
        public ShortLeaveSetupController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown, ILeaveTransactionBL leave = null)
        {
            _mediator = mediator;
            _global = global;
            _dropdown = dropdown;
            _leave = leave;
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
            ViewBag.Employees = await _mediator.Send(new GetAllEmployeeQuery() { CompId = compId });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<Domains.Models.ShortLeaveSetup> SLeaveSetup)
        {
            try
            {

                await _mediator.Send(new UpsertShortLeaveSetupCommand() { ShortLeaveSetup = SLeaveSetup });

                //if (SLeaveSetup.FirstOrDefault().ShortLeaveSetupId != 0)
                //{
                //    return Json(new BLStatus { Message = $@"Data Update Successfully" });
                //}
                return Json(new BLStatus { Message = $@"Data Save Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.Message });
            }
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByEmpId(int empId)
        {
            try
            {
                if (empId < 1) return BadRequest();

                var data = await _mediator.Send(new GetShortLeaveSetupDataByEmpIdQuery { EmpId = empId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int ShortLeaveSetupId)
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
                var status = await _leave.Delete(ShortLeaveSetupId);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteByEmpId(List<int> empIds)
        {
            try
            {
                var status = await _leave.DeleteByEmpId(empIds);
                return Json("");
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }
    }
}
