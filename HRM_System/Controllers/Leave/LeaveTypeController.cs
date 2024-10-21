using Application.Tasks.Commands.CLeaveType;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QLeaveType;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers.Leave
{
    [Authorize]
    public class LeaveTypeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly Helper.CommonDropdown _dropdown;

        public LeaveTypeController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
        {
            _mediator = mediator;
            _global = global;
            _dropdown = dropdown;
        }


        //public async Task<IActionResult> Index()
        //{
        //    #region Access
        //    var roleid = _global.GetRoleID();
        //    var controller = RouteData.Values["controller"];
        //    var action = RouteData.Values["action"];
        //    var url = $"{controller}/{action}";
        //    ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
        //    ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
        //    ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
        //    ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
        //    #endregion

        //    var ClientId = _global.GetClientId();
        //    var orgid = _global.GetOrgId();
        //    ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, ClientId);

        //    return View();
        //}

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

            var ClientId = _global.GetClientId();
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = orgid;
            ViewBag.OrgDropdown = await _dropdown.OrganisationDropdown(orgid, ClientId);

            return View();
        }

        public async Task<IActionResult> loaddata()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
            var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var search = Request.Form["search[value]"].FirstOrDefault();
            var orgid = _global.GetOrgId();  // Get the OrgId
            var totalrecord = 0;

            // Pass orgid to your query
            var data = await _mediator.Send(new GetAllLeaveTypeQuery { OrgId = orgid });

            if (data != null)
            {
                totalrecord = data.Count;
            }

            return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = data });
        }


        //public async Task<IActionResult> loaddata()
        //{
        //    var draw = Request.Form["draw"].FirstOrDefault();
        //    var start = Request.Form["start"].FirstOrDefault();
        //    var length = Request.Form["length"].FirstOrDefault();
        //    var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
        //    var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //    var search = Request.Form["search[value]"].FirstOrDefault();
        //    var totalrecord = 0;

        //    var data = await _mediator.Send(new GetAllLeaveTypeQuery());


        //    if (data.Count != 0)
        //    {
        //        if (data != null)
        //        {
        //            totalrecord = data.Count;
        //        }
        //        else
        //        {
        //            totalrecord = 0;
        //        }
        //    }
        //    return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = data });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(LeaveType leaveType)
        {
            try
            {
                if (leaveType.LeaveTypeId > 0)
                {
                    await _mediator.Send(new UpdateLeaveTypeCommand() { LeaveType = leaveType });

                    var json = JsonConvert.SerializeObject(leaveType);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = leaveType.LeaveTypeId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} LeaveType", DocumentReferance = json });

                    return Json(new BLStatus { Message = "Data Update Successfully." });
                }
                else
                {
                    await _mediator.Send(new CreateLeaveTypeCommand() { LeaveType = leaveType });

                    var json = JsonConvert.SerializeObject(leaveType);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = leaveType.LeaveTypeId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} LeaveType", DocumentReferance = json });

                    return Json(new BLStatus { Message = "Data Save Successfully." });
                }
            }
            catch (Exception)
            {
                return Json(new BLStatus { Message = "Uable To Save/Update Record.", IsError = true });
            }                       
        }
        public async Task<IActionResult> Edit (int id)
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
            var data = await _mediator.Send(new GetAllLeaveTypeByIdQuery() { LeaveTypeId = id });
            return View("index", data);
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
                var data = await _mediator.Send(new GetAllLeaveTypeByIdQuery() { LeaveTypeId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteLeaveTypeCommand() { LeaveTypeId = id });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} LeaveType", DocumentReferance = id.ToString() });
                }
                return Json(new BLStatus { Message = "Delete Data Successfully" });
            }
            catch (Exception)
            {
                return Json(new BLStatus { Message = "Uable To Delete Record.", IsError = true });
            }
            
        }
    }
}
