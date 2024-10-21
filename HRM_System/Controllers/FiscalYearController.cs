using Application.Tasks.Commands.CFiscalYear;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QFiscalYear;
using UKHRM.Helpers;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Utility;
using Newtonsoft.Json;

namespace UKHRM.Controllers
{
    [Authorize]
    public class FiscalYearController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;
        public FiscalYearController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
        {
            _mediator = mediator;
            _global = global;
            _dropdown = dropdown;
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
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
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
            var totalrecord = 0;

            var data = await _mediator.Send(new GetAllQuery());


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
        public async Task<IActionResult> create(FiscalYear fiscalYear)
        {
            try
            {
                if (fiscalYear.FiscalYearId > 0)
                {
                    await _mediator.Send(new UpdateFiscalYearCommand() { FiscalYear = fiscalYear });

                    var json = JsonConvert.SerializeObject(fiscalYear);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = fiscalYear.FiscalYearId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Upsert} FiscalYear", DocumentReferance = json });
                    return Json(new BLStatus { Message = "Data Update Successfully." });
                }
                else
                {

                    await _mediator.Send(new CreateFiscalYearCommand() { FiscalYear = fiscalYear });

                    var json = JsonConvert.SerializeObject(fiscalYear);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = fiscalYear.FiscalYearId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Upsert} FiscalYear", DocumentReferance = json });

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
            var data = await _mediator.Send(new GetByIdQuery() { FiscalYearId = id });
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
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
                var data = await _mediator.Send(new GetByIdQuery() { FiscalYearId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteFiscalYearCommand() { FiscalYearId = id });

                    var json = JsonConvert.SerializeObject(id);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} FiscalYear", DocumentReferance = json });
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
