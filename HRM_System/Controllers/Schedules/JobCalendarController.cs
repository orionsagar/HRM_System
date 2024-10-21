using Application.Tasks.Commands.CJobCalendar;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QJobCalendar;
using UKHRM.Helpers;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class JobCalendarController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Helper.CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;

        public JobCalendarController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global)
        {
            _mediator = mediator;
            _dropdown = dropdown;
            _global = global;
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
            ViewBag.calendarlist = await _mediator.Send(new GetAllJobCalendarQuery() { OrgId = orgid});
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(JobCalender calender)
        {
            try
            {
                if (calender.JobCalenderId != 0)
                {
                    await _mediator.Send(new UpdateJobCalendarCommand() { JobCalender = calender });

                    var json = JsonConvert.SerializeObject(calender);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = calender.JobCalenderId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} JobCalender", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreateJobCalendarCommand() { JobCalender = calender });

                    var json = JsonConvert.SerializeObject(calender);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = calender.JobCalenderId.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} JobCalender", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Something Wrong";
                return View(calender);
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
            ViewBag.Action = "Edit";
            var shift = await _mediator.Send(new GetByIdQuery() { JobCaleanarId = id });
            ViewBag.calendarlist = await _mediator.Send(new GetAllJobCalendarQuery());
            ViewBag.ScheduleId = await _dropdown.ScheduleDropdown(ComId);
            return View("Index", shift);
        }

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
                var data = await _mediator.Send(new GetByIdQuery() { JobCaleanarId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteJobCalendarCommand() { JobCalendarId = id });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} JobCalender", DocumentReferance = id.ToString() });
                }
                return Json(new BLStatus { Message = "JobCalender Delete Successfully", StatusCode = "200" } );
            }
            catch (Exception ex)
            {
                //ViewBag.ErrorMsg = "Unable to delete";
                return Json(new BLStatus { Message = "JobCalender Unable to delete", IsError = true });
            }            
        }
    }
}
