using Application.Tasks.Commands.CHoliday;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QHoliday;
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
    public class HolidayController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;

        public HolidayController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global)
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
            var CompId = _global.GetCompID();
            var orgid = _global.GetOrgId();
            ViewBag.JobCalendarId = await _dropdown.JobCalendarDropdown(CompId, orgid);
            return View();  
        }

        public async Task<IActionResult> Create()
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
            var CompId = _global.GetCompID();
            var orgid = _global.GetOrgId();
            ViewBag.JobCalendarId = await _dropdown.JobCalendarDropdown(CompId, orgid);
            
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(List<Holiday> holiday)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in holiday)
                    {
                        if (item.HolidayId > 0)
                        {
                            await _mediator.Send(new UpdateHolidayCommand() { Holiday = item });

                            var json = JsonConvert.SerializeObject(holiday);

                            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = string.Join(",", holiday.Select(h => h.HolidayId)), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Holiday", DocumentReferance = json });
                        }
                        else
                        {
                            await _mediator.Send(new CreateHolidayCommand() { Holiday = item });

                            var json = JsonConvert.SerializeObject(holiday);

                            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = string.Join(",", holiday.Select(h => h.HolidayId)), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Holiday", DocumentReferance = json });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                          
            }
            return Json(new BLStatus { Message = "Holiday Save Successfully" });
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
                var data = await _mediator.Send(new GetHolidayByIdQuery() { HolidayId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteHolidayCommand() { HolidayId = id });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} Holiday", DocumentReferance = id.ToString() });
                }
                return Json(new BLStatus { Message = "Delete Holiday Parmanently" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = "Unable To Delete" });
            }
            
        }

        public async Task<IActionResult> GetHolidays(int JobCalendarId,int CompId, int Year,int Month)
        {
            var data = await _mediator.Send(new GetHolidaysQuery() { JobCalendarId = JobCalendarId, CompId = CompId, Year = Year, Month = Month });
            return Json(data);
        }
    }
}
