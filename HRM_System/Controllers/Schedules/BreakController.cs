using Application.Tasks.Commands.CBreak;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QBreak;
using UKHRM.Helpers;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class BreakController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;

        public BreakController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
        {
            _mediator = mediator;
            _global = global;
            _dropdown = dropdown;
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
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.BreakList = await _mediator.Send(new GetAllBreakQuery());
            ViewBag.ScheduleId = await _dropdown.ScheduleDropdown(ComId);
            ViewBag.ShiftId = await _dropdown.ShiftDropdown(ComId, OrgId);
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(OrgId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Break @break)
        {
            try
            {
                if (@break.BreakId > 0)
                {
                    await _mediator.Send(new UpdateBreakCommand() { Break = @break });

                    var json = JsonConvert.SerializeObject(@break);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = @break.BreakId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Break", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreateBreakCommand() { Break = @break });

                    var json = JsonConvert.SerializeObject(@break);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = @break.BreakId.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Break", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
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
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.Action = "Edit";
            var shift = await _mediator.Send(new GetBreakByIdQuery() { BreakId = id });
            ViewBag.BreakList = await _mediator.Send(new GetAllBreakQuery());
            ViewBag.ScheduleId = await _dropdown.ScheduleDropdown(ComId);
            ViewBag.ShiftId = await _dropdown.ShiftDropdown(ComId, OrgId);
            return View("Index", shift);
        }
        //[HttpPost]
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
                var data = await _mediator.Send(new GetBreakByIdQuery() { BreakId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteBreakCommand() { BreakId = id });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Break", DocumentReferance = id.ToString() });
                }
                return Json(new BLStatus { Message = "Shift Delete Successfully", IsError = false });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = "Shift Alredy Asign. Unable to delete.😨😨", IsError = true });
            }

        }
    }
}
