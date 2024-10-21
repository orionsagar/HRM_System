using Application.Tasks.Commands.CLeaveSetup;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QEmployee;
using Application.Tasks.Queries.QFiscalYear;
using Application.Tasks.Queries.QLeaveSetup;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
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

namespace UKHRM.Controllers.Leave
{
    [Authorize]
    public class LeaveSetupController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;
        public LeaveSetupController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
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
            var compId = _global.GetCompID();
            ViewBag.Employees = await _mediator.Send(new GetAllEmployeeQuery() { CompId = compId, OrgId = orgid });
            ViewBag.FiscalYear = await _dropdown.FiscalYearDropdown(compId);
            ViewBag.LeaveType = await _dropdown.LeaveTypeDropdownForAppendGrid(orgid);
            var clientid = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientid);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(EmpLeaveFilter filter)
        {
            try
            {
                var OrgId = _global.GetOrgId();
                filter.OrgId = OrgId;
                var data = await _mediator.Send(new GetEmployeeByLeaveQuery { LeaveFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveSutupVM leaveSetup)
        {
            try
            {
                List<LeaveSetup> leaveSetups = new List<LeaveSetup>();
                var comId = _global.GetCompID();
                var userId = _global.GetUserID();
                foreach (int empId in leaveSetup.EmpIds)
                {
                    foreach (var item in leaveSetup.LeaveTypes) 
                    {
                        leaveSetups.Add(new LeaveSetup
                        {
                            AddedBy = userId,
                            CompId = comId,
                            OrgId = leaveSetup.OrgId,
                            AddedDate = DateTime.Now,
                            EmpId = empId,
                            LeaveTypeId = item.LeaveTypeId,
                            LeaveDays = item.LeaveDays,
                            Remark = item.Remark,
                            FiscalYearId = leaveSetup.FiscalYearId,
                        });
                    }

                }
                await _mediator.Send(new UpsertLeaveSetupCommand() { leaveSetups = leaveSetups });

                var json = JsonConvert.SerializeObject(leaveSetups);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = string.Join(",", leaveSetups.Select(ls => ls.LeaveSetupId)), CommandType = Enums.commandtype.Upsert.ToString(), TransStatement = $"{Enums.commandtype.Upsert} LeaveSetup", DocumentReferance = json });

                return Json(new BLStatus { Message = $@"Data Save Successfully" });
            }
            catch (Exception ex)
            {

                return Json(new BLStatus {IsError=true, Message = ex.Message });
            }
            
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(List<LeaveSetup> leaveSetup)
        //{

        //    await _mediator.Send(new UpsertLeaveSetupCommand() { leaveSetups = leaveSetup });

        //    return Json(new BLStatus { Message = $@"Data Save Successfully" });
        //}

        public async Task<IActionResult> GetEmployeeLeave(int EmpId, int fiscalYearId)
        {
            var leaves = await _mediator.Send(new GetLeaveByEmpIdQuery() { EmpId = EmpId, FiscalYearId=fiscalYearId });
            return Json(leaves);
        }
        public async Task<IActionResult> GetLeaveDaysById(int LeaveTypeId)
        {
            var leaves = await _global.GetLeaveDaysByLeaveTypeId(LeaveTypeId);
            return Json(leaves);
        }

    }
}
