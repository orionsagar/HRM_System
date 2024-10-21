using Application.Tasks.Commands.CCompany;
using Application.Tasks.Commands.CDepartment;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QCompany;
using Application.Tasks.Queries.QDepartment;
using Application.Tasks.Queries.QEmployee;
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

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class CompanyRuleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;

        public CompanyRuleController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
        {
            this._mediator = mediator;
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
            var CompID = _global.GetCompID();
            ViewBag.RuleDefineds = await _mediator.Send(new GetRuleDefindQuery());
            ViewBag.CompanyRules = await _mediator.Send(new GetCompanyRuleQuery() { CompId=CompID});
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<CompanyRule> rules)
        {
            try
            {
                var compId = _global.GetCompID();
                var userId = _global.GetUserID();
                var orgid = _global.GetOrgId();
                var clientid = _global.GetClientId();
                if (ModelState.IsValid)
                {
                    foreach (var item in rules)
                    {
                        item.AddedDate = DateTime.Now;
                        item.UpdateDate = DateTime.Now.Date;
                        item.EffectiveDate = DateTime.Now.Date;                        
                        item.CompId = compId;
                        item.ClientID = clientid;
                        item.AddedBy = userId;
                        item.UpdateBy = userId;
                    }
                    await _mediator.Send(new CreateCompanyRulesCommand { CompanyRules = rules, CompId = compId });

                    var json = JsonConvert.SerializeObject(rules);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = string.Join(",", rules.Select(r => r.RuleID)), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = $"{Enums.commandtype.Create} CompanyRule", DocumentReferance = json });

                    return Json(new BLStatus { Message = "👌👌👌 Company rules assigned." });
                }
                else
                {
                    return Json(new BLStatus {IsError=true, Message = "🥺🥺🥺 Submited data is not valid" });
                }
            }
            catch (Exception ex)
            {
                return Json(new BLStatus {IsError=true, Message = $"😒 {ex.Message}" });
            }
           
        }
      
    }
}
