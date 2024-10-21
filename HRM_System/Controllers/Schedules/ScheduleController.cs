using Application.Tasks.Commands.CSchedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domains.Models;
using UKHRM.Helper;
using Application.Tasks.Queries.QSchedule;
using Microsoft.AspNetCore.Authorization;
using UKHRM.Helpers;
using Newtonsoft.Json;
using Application.Tasks.Commands.CTransactionLog;
using Utility;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly Helper.CommonDropdown _dropdown;

        public ScheduleController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
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
            var orgid = _global.GetOrgId();
            ViewBag.ScheduleList = await _mediator.Send(new GetAllScheduleQuery() { OrgId = orgid});
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            if(schedule.ScheduleId > 0)
            {
                await _mediator.Send(new UpdateScheduleCommand() { Schedule = schedule });

                var json = JsonConvert.SerializeObject(schedule);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = schedule.ScheduleId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Schecule", DocumentReferance = json });
            }
            else
            {
                await _mediator.Send(new CreateScheduleCommand() { Schedule = schedule });

                var json = JsonConvert.SerializeObject(schedule);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = schedule.ScheduleId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Schecule", DocumentReferance = json });
            }            
            return RedirectToAction(nameof(Index));
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
            ViewBag.Action = "Edit";
            var schedule = await _mediator.Send(new GetScheduleByIdQuery() { ScheduleId = id });
            ViewBag.ScheduleList = await _mediator.Send(new GetAllScheduleQuery());
            return View("Index", schedule);
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
                var data = await _mediator.Send(new GetScheduleByIdQuery() { ScheduleId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteScheduleCommand() { ScheduleId = id });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} Schecule", DocumentReferance = id.ToString() });
                }
                return Json(new BLStatus { Message = "Schedule Delete Successfully", IsError = false });
            }
            catch (Exception)
            {
                return Json(new BLStatus { Message = "Schedule Already Asign. Unable to delete.", IsError = true });
            }
            
        }
    }
}
