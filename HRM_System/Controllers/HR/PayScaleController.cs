using Application.Tasks.Commands.CPayScale;
using Application.Tasks.Queries.QPayScale;
using AutoMapper;
using UKHRM.Helpers;
using Business.HR;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class PayScaleController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPayScaleBL _payScaleBL;
        //private readonly ILogger _logger;

        public PayScaleController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IPayScaleBL payScaleBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _payScaleBL = payScaleBL;
            _mapper = mapper;
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
            return View();
        }
        public async Task<IActionResult> LoadData()
        {
            try
            {
                //var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var OrgId = _global.GetOrgId();
                var ClientId = _global.GetClientId();
                var payScale = await _mediator.Send(new SP_Dt_PayScaleListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, ComId = comid, OrgId = OrgId, ClientId = ClientId, Search = search  });

                //total number of rows counts   
                if (payScale.Count > 0)
                {
                    recordsTotal = payScale.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = payScale });
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                //_logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
                throw;
            }
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
            var rulegroup = "Salary Break Down";
            var rulecode = "SB-003";
            ViewBag.Action = "Add";
            var CompId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.PayScaleTypeId = await _dropdown.PayscaleTypeDropdown(CompId,OrgId);
            ViewBag.RuleID = await _dropdown.RulesDropdown(rulegroup, rulecode);
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PayScaleVM payScaleVM)
        {
            try
            {
                var result = await _payScaleBL.Add(payScaleVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var rulegroup = "Salary Break Down";
                var rulecode = "SB-003";
                ViewBag.Action = "Create";
                var CompId = _global.GetCompID();
                var OrgId = _global.GetOrgId();
                ViewBag.PayScaleTypeId = await _dropdown.PayscaleTypeDropdown(CompId, OrgId);
                ViewBag.RuleID = await _dropdown.RulesDropdown(rulegroup, rulecode);
                throw new Exception(ex.Message);
            }


        }

        public async Task<IActionResult> Edit(int Id)
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
            var rulegroup = "Salary Break Down";
            var rulecode = "SB-003";
            var CompId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.PayScaleTypeId = await _dropdown.PayscaleTypeDropdown(CompId, OrgId);
            var payscale = await _mediator.Send(new GetByIdQuery() { PayScaleId = Id });
            //var sbd = await _mediator.Send(new GetSalaryBreakDownByPayScaleIdQuery() { PayScaleId = Id });
            var payscalvm = _mapper.Map<PayScaleVM>(payscale);
            if (payscale?.SalaryBreakDown?.RuleID == 23)
            {
                ViewBag.RuleID = await _dropdown.RulesDropdown(rulegroup, rulecode, payscale?.SalaryBreakDown?.RuleID ?? 0);
            }
            else
            {
                ViewBag.RuleID = await _dropdown.GrossRulesDropdown(rulegroup, rulecode, payscale?.SalaryBreakDown?.RuleID ?? 0);
            }

            return View("Create", payscalvm);
        }

        public async Task<IActionResult> GetLebelIsGovtOrNot(int PayScaleTypeID)
        {
            var lebel = await _mediator.Send(new GetLebelIsGovtOrNotQuery() { PayScaleTypeId = PayScaleTypeID });
            return Json(lebel);
        }

        public async Task<IActionResult> GetPayScaleByDesigId(int desigId)
        {
            var payScale = await _mediator.Send(new GetPayScaleByDesigIdQuery() { DesigId = desigId });
            return Json(payScale);
        }

        public async Task<IActionResult> BindBasicDropdown()
        {
            var rulegroup = "Salary Break Down";
            var rulecode = "SB-003";
            var RuleID = await _dropdown.RulesDropdown(rulegroup, rulecode);
            return Json(RuleID);
        }

        public async Task<IActionResult> BindGrossDropdown()
        {
            var rulegroup = "Salary Break Down";
            var rulecode = "SB-003";
            var RuleID = await _dropdown.GrossRulesDropdown(rulegroup, rulecode); // here rulecode use for not in condition
            return Json(RuleID);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                //#region Access
                //var roleid = _global.GetRoleID();
                //var controller = RouteData.Values["controller"];
                //var action = RouteData.Values["action"];
                //var url = $"{controller}/{action}";
                //ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
                //ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
                //ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
                //ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
                //#endregion
                //var tableData = await _mediator.Send(new GetByIdQuery { PayScaleId = Convert.ToInt32(Id) });

                //if (tableData != null)
                //{
                    var result = await _payScaleBL.Delete((int)Id);
                    
                //}
                return Json(new BLStatus { Message = "Data Successfully Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }
    }
}
