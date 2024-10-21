using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Application.Tasks.Commands.CShift;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QSchedule;
using Application.Tasks.Queries.QShift;
using UKHRM.Helpers;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Newtonsoft.Json;
using Utility;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class ShiftController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;

        public ShiftController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global)
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
            var ComId = _global.GetCompID();
            var orgid = _global.GetOrgId();
            ViewBag.ShiftList = await _mediator.Send(new GetAllQuery() { OrgId = orgid});
            ViewBag.ScheduleId = await _dropdown.ScheduleDropdown(orgid);
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Shift shift)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (shift.Has2Date == 0)
                    //{
                    //    var duratin = shift.Endtime - shift.StartTime;
                    //    shift.ShiftDuration = duratin.Hours;
                    //}
                    //else
                    //{
                    //    var today = DateTime.Now;
                    //    var start = today.Add(shift.StartTime);
                    //    var end = today.AddDays(1).Add(shift.Endtime);

                    //    var duratin = end - start;
                    //    shift.ShiftDuration = duratin.Hours;
                    //}

                    //var today = DateTime.Now;
                    //var start = today.Add(shift.StartTime);
                    //var end = today.Add(shift.Endtime);

                    //if (shift.Has2Date>0)
                    //{
                    //    end = end.AddDays(1);
                    //}

                    //var duratin = end - start;
                    var duratin = TimeDuration(shift.StartTime, shift.Endtime, shift.Has2Date);
                    shift.ShiftDuration = duratin.Hours;

                    // check duration 
                    if (shift.ShiftDuration < 1)
                    {
                        var ComId = _global.GetCompID();
                                      
                        ViewBag.ShiftList = await _mediator.Send(new GetAllQuery());
                        ViewBag.ScheduleId = await _dropdown.ScheduleDropdown(ComId,shift.ScheduleId);

                        ModelState.AddModelError(String.Empty, "Start time should be less than End time!!!");
                        return View(nameof(Index),shift);
                    }

                    if (shift.ShiftId != 0)
                    {
                        await _mediator.Send(new UpdateShiftCommand() { Shift = shift });

                        var json = JsonConvert.SerializeObject(shift);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = shift.ShiftId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Shift", DocumentReferance = json });
                    }
                    else
                    {
                        await _mediator.Send(new CreateShiftCommand() { Shift = shift });

                        var json = JsonConvert.SerializeObject(shift);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = shift.ShiftId.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Shift", DocumentReferance = json });
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Submited data is not valid!!");
                    return View(nameof(Index), shift);
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Something Wrong";
                return View(nameof(Index), shift);
                throw;
            }            
        }

        TimeSpan TimeDuration(TimeSpan startTime, TimeSpan endTime, int has2Date)
        {
            var today = DateTime.Now;
            var start = today.Add(startTime);
            var end = today.Add(endTime);

            if (has2Date > 0)
            {
                end = end.AddDays(1);
            }
            return end - start;
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
            var shift = await _mediator.Send(new GetByIdQuery() { ShiftId = id });
            ViewBag.ShiftList = await _mediator.Send(new GetAllQuery());
            ViewBag.ScheduleId = await _dropdown.ScheduleDropdown(ComId);
            var ClientId = _global.GetClientId();
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, ClientId);
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
                var data = await _mediator.Send(new GetByIdQuery() { ShiftId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteShiftCommand() { ShiftId = id });

                    var json = JsonConvert.SerializeObject(id);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} Shift", DocumentReferance = json });
                }
                return Json(new BLStatus { Message = "Shift Delete Successfully", IsError = false });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = "Shift Already Asign", IsError = true });
            }
            
        }

        public async Task<IActionResult> GetShiftByScheduleId(int ScheduleId)
        {
            var ComId = _global.GetCompID();
            var data = await _mediator.Send(new ShiftDropdownByScheduleIdQuery() { CompId = ComId, ScheduleId = ScheduleId });
            return Json(data);
        }
    }
}
