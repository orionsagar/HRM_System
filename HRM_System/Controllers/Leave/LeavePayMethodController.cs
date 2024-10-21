using Application.Tasks.Commands.CLeavePayMethod;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QLeavePayMethod;
using UKHRM.Helpers;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Utility;

namespace UKHRM.Controllers.Leave
{
    [Authorize]
    public class LeavePayMethodController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;

        public LeavePayMethodController(IMediator mediator, IGlobalHelper global)
        {
            _mediator = mediator;
            _global = global;
        }
        public async Task<IActionResult> IndexAsync()
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

            var data = await _mediator.Send(new GetAllQuery{ OrgId =  orgid});


            if (data.Count != 0)
            {
                if (data != null)
                {
                    totalrecord = data.Count;
                }
                else
                {
                    totalrecord = 0;
                }
            }
            return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = data });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(LeavePayMethod leavePayMethod)
        {
            try
            {
                if (leavePayMethod.LeavePayMethodId > 0)
                {
                    await _mediator.Send(new UpdateLeavePayMethodCommand() { LeavePayMethod = leavePayMethod });

                    var json = JsonConvert.SerializeObject(leavePayMethod);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = leavePayMethod.LeavePayMethodId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} LeavePayMethod", DocumentReferance = json });

                    return Json(new BLStatus { Message = "Data Update Successfully.", Data = leavePayMethod.LeavePayMethodId });
                }
                else
                {
                    await _mediator.Send(new CreateLeavePayMethodCommand() { LeavePayMethod = leavePayMethod });

                    var json = JsonConvert.SerializeObject(leavePayMethod);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = leavePayMethod.LeavePayMethodId.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} LeavePayMethod", DocumentReferance = json });

                    return Json(new BLStatus { Message = "Data Save Successfully." });
                }
            }
            catch (Exception)
            {
                return Json(new BLStatus { Message = "Uable To Save/Update Record.", IsError = true });
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
            var data = await _mediator.Send(new GetByIdQuery() { LeavPayMethodId = id });
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
                var data = await _mediator.Send(new GetByIdQuery() { LeavPayMethodId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteLeavePayMethodCommand() { LeavePayMethodId = id });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} LeavePayMethod", DocumentReferance = id.ToString() });
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
