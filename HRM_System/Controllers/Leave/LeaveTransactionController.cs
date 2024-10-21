using Application.Tasks.Commands.CLeaveTransaction;
using Application.Tasks.Queries.QLeaveTransaction;
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
using System.Linq;
using System.Threading.Tasks;

namespace UKHRM.Controllers.Leave
{
    [Authorize]
    public class LeaveTransactionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly ILeaveTransactionBL _leaveTransactionBL;

        public LeaveTransactionController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global, ILeaveTransactionBL leaveTransactionBL)
        {
            _mediator = mediator;
            _dropdown = dropdown;
            _global = global;
            _leaveTransactionBL = leaveTransactionBL;
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
            var ClientId = _global.GetClientId();
            ViewBag.LeaveTypeId = await _dropdown.LeaveTypeDropdown(ComId, OrgId);
            ViewBag.LeavePayMethodId = await _dropdown.LeavePayMethodDropdown(ComId, OrgId);
            ViewBag.FiscalYearId = await _dropdown.FiscalYearDropdown(ComId);
            ViewBag.EmpId = await _dropdown.EmployeeDropdown(0,OrgId, ClientId);
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var dtFrom = Request.Form["dtFrom"].FirstOrDefault();
                var dtTo = Request.Form["dtTo"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var search = Request.Form["search[value]"].FirstOrDefault();
                var totalrecord = 0;
                var UserId = _global.GetUserID(); ;
                var ComId = _global.GetCompID();
                var OrgId = _global.GetOrgId();
                var dtfrom = _global.DateConvertion(dtFrom);
                var dtto = _global.DateConvertion(dtTo);
                var leaves = new List<LeaveTransactionVM>();

                leaves = await _mediator.Send(new SP_Dt_LeaveTransactionListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = ComId, DtFrom = dtfrom, DtTo = dtto, OrgId = OrgId });


                if (leaves.Count != 0)
                {
                    if (leaves.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = leaves.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = leaves });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> LeaveApprovalList()
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
            var ClientId = _global.GetClientId();
            ViewBag.LeaveTypeId = await _dropdown.LeaveTypeDropdown(ComId, OrgId);
            ViewBag.LeavePayMethodId = await _dropdown.LeavePayMethodDropdown(ComId, OrgId);
            ViewBag.FiscalYearId = await _dropdown.FiscalYearDropdown(ComId);
            ViewBag.EmpId = await _dropdown.EmployeeDropdown(0, OrgId, ClientId);
            return View();
        }

        public IActionResult GetLeavDay(string DtStart, string DtEnd)
        {
            var StartDate = _global.DateConvertionCSharp(DtStart);
            var EndDate = _global.DateConvertionCSharp(DtEnd);
            DateTime dtfrom = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
            DateTime dtto = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day);
            //var day = EndDate.Day - StartDate.Day + 1;
            var day = (EndDate - StartDate).TotalDays + 1;
            return Json(day);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(LeaveTransaction leaveTransaction, int FiscalYearId)
        {
            leaveTransaction.OrgId = _global.GetOrgId();
            var message = await _leaveTransactionBL.Upsert(leaveTransaction, leaveTransaction.LeaveTransactionId, FiscalYearId, _global.GetOrgId());
            return Json(message);
        }
        public async Task<IActionResult> ApproveLeave(int leaveTransactionId, string status)
        {            
            var message = await _leaveTransactionBL.LeaveApproved(leaveTransactionId, status);
            return RedirectToAction(nameof(LeaveApprovalList));
        }

        public async Task<IActionResult> GetLeaveDetails(int EmpId, int FiscalYearId, int LeaveTypeId)
        {
            var OrgId = _global.GetOrgId();
            var leaveinfo = await _mediator.Send(new SP_LeaveDetailsQuery() { EmpId = EmpId, FiscalYearId = FiscalYearId, LeaveTypeId = LeaveTypeId, OrgId = OrgId });
            return Json(leaveinfo);
        }
        public async Task<IActionResult> GetLeaveTransactionDataById(int leaveTransactionId)
        {
            var leaveinfo = await _mediator.Send(new GetLeaveTransactionByIdQuery() { LeaveTransactionId = leaveTransactionId});
            return Json(leaveinfo);
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
                var data = await _mediator.Send(new GetLeaveTransactionByIdQuery() { LeaveTransactionId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteLeaveTransactionCommand() { LeaveTransactionId = id });
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
