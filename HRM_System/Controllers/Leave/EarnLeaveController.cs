using Application.Tasks.Commands.CEarnLeave;
using Application.Tasks.Queries.QEarnLeave;
using Application.Tasks.Queries.QFiscalYear;
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
    public class EarnLeaveController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IEarnLeaveBL _earnLeaveBL;

        public EarnLeaveController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global, IEarnLeaveBL earnLeaveBL)
        {
            this._mediator = mediator;
            _dropdown = dropdown;
            _global = global;
            _earnLeaveBL = earnLeaveBL;
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
            ViewBag.SectionId = await _dropdown.SectionDropdown(ComId, OrgId);
            ViewBag.DepartmentId = await _dropdown.DepartmentDropdown(ComId, OrgId);
            ViewBag.FiscalYearId = await _dropdown.FiscalYearDropdown(ComId, OrgId);
            ViewBag.EmpId = await _dropdown.EmployeeDropdown(ComId);
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(OrgId);
            return View();
        }

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var SectionId = Request.Form["SectionId"].FirstOrDefault();
                var DepartmentId = Request.Form["DepartmentId"].FirstOrDefault();
                var FiscalYearId = Request.Form["FiscalYearId"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var search = Request.Form["search[value]"].FirstOrDefault();
                var totalrecord = 0;
                var UserId = DataEncryption.DecryptString(Request.Cookies["UserID"]);
                //var Role = DataEncryption.DecryptString(Request.Cookies["Role"]);
                var ComId = _global.GetCompID();
                var earnLeaves = new List<EarnLeaveVM>();

                earnLeaves = await _mediator.Send(new SP_EarnLeaveQuery() { CardNo = "all", SectId = Convert.ToInt32(SectionId), DeptId = Convert.ToInt32(DepartmentId), DesigId = 0, FloorId = 0, LineId = 0, CompId = ComId, FiscalYearId = Convert.ToInt32(FiscalYearId) });


                if (earnLeaves.Count != 0)
                {
                    if (earnLeaves.Count != 0)
                    {
                        totalrecord = earnLeaves.Count;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = earnLeaves });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EarnLeave earnLeave)
        {
            try
            {
                var message = await _earnLeaveBL.Upsert(earnLeave);
                return Json(message);
            }
            catch (Exception)
            {
                var ComId = _global.GetCompID();
                var OrgId = _global.GetOrgId();
                ViewBag.SectionId = await _dropdown.SectionDropdown(ComId, OrgId);
                ViewBag.DepartmentId = await _dropdown.DepartmentDropdown(ComId, OrgId);
                ViewBag.FiscalYearId = await _dropdown.FiscalYearDropdown(ComId, OrgId);
                ViewBag.EmpId = await _dropdown.EmployeeDropdown(ComId);
                return Json(earnLeave);
            }
        }
        public async Task<IActionResult> CloseFiscalYear(int CurrFiscalYearId, int NewFiscalYearId)
        {
            var el = new EarnLeaveVM
            {
                FiscalYearId = NewFiscalYearId,
                OldFiscalYearId = CurrFiscalYearId,
                UserId = Convert.ToInt32(_global.GetUserID()),
                AddDate = DateTime.Now,
                CompId = _global.GetCompID()
            };
            await _mediator.Send(new SP_ClosingFiscalYearQuery() { earnLeave = el });
            return Json("Successfully Cloased FiscalYear");
        }
        public async Task<IActionResult> UpdateEarnLeave(int FiscalYearId)
        {
            var CompId = _global.GetCompID();
            await _mediator.Send(new SP_UpdateEarnLeaveQuery() { CompId = CompId,FiscalYearId = FiscalYearId });
            return Json("Update FiscalYear Successfully");
        }

        public async Task<IActionResult> GetYearInfo(int id)
        {
            var leavedata = await _mediator.Send(new GetByIdQuery() { FiscalYearId = id });
            var date = new
            {
                startdate = leavedata.startdate.ToString("yyyy-MM-dd"),
                enddate = leavedata.enddate.ToString("yyyy-MM-dd")
            };
            return Json(date);
        }
        public async Task<IActionResult> GetEarnLeaveDataById(int EarnLeaveId)
        {
            var leavedata = await _mediator.Send(new GetEarnLeaveByIdQuery() { EarnLeaveId = EarnLeaveId });
            return Json(leavedata);
        }

        [HttpPost]
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
                var data = await _mediator.Send(new DeleteEarnLeaveCommand() { EarnLeaveId = id });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
