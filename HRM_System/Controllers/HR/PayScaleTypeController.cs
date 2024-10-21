using Application.Tasks.Commands.CPayScaleType;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QPayScaleType;
using UKHRM.Helpers;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class PayScaleTypeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly ILogger<SubModuleController> _logger;
        private readonly CommonDropdown _dropdown;

        public PayScaleTypeController(IMediator mediator, ILogger<SubModuleController> logger, CommonDropdown dropdown, IGlobalHelper global)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
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
            return View();
        }

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
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
                var payScaleTypes = await _mediator.Send(new SP_Dt_PayScaleTypeListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection,Search = search, ComId = comid, OrgId = OrgId, ClientId = ClientId });

                //total number of rows counts   
                if (payScaleTypes.Count > 0)
                {
                    recordsTotal = payScaleTypes.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = payScaleTypes });
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                _logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
                throw;
            }
        }
        public async Task<IActionResult> CreateAsync()
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
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            ViewBag.Action = "Add";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PayScaleType payScaleType)
        {
            if (ModelState.IsValid)
            {
                if(payScaleType.PayScaleTypeId > 0)
                {
                    await _mediator.Send(new EditPayScaleTypeCommand() { PayScaleType = payScaleType });

                    var json = JsonConvert.SerializeObject(payScaleType);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = payScaleType.PayScaleTypeId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Update), TransStatement = $"{Enum.GetName(Enums.commandtype.Create)} PayScaleType", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreatePayScaleTypeCommand() { PayScaleType = payScaleType });

                    var json = JsonConvert.SerializeObject(payScaleType);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = payScaleType.PayScaleTypeId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = $"{Enums.commandtype.Create} PayScaleType", DocumentReferance = json });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
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
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);

            if (id == null)
            {
                return Redirect("/error/404");
            }
            var pstype = await _mediator.Send(new GetPayScaleTypeByIdQuery { PayScaleTypeId = Convert.ToInt32(id) });

            if (pstype == null)
            {
                return Redirect("/error/404");
            }
            ViewBag.Action = "Edit";
            return View("Create", pstype);
        }
        public async Task<JsonResult> Delete(int? Id)
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
                var tableData = await _mediator.Send(new GetPayScaleTypeByIdQuery { PayScaleTypeId = Convert.ToInt32(Id) });

                if (tableData != null)
                {
                    await _mediator.Send(new DeletePayScaleTypeCommand() { PayScaleTypeId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} PayScaleType", DocumentReferance = Id.ToString()});
                }
                return Json(new BLStatus { Message = "PayScaleType Delete Successfully", IsError = false });
            }
            catch (Exception)
            {
                return Json(new BLStatus { Message = "PayScaleType Already Asign. Unable to delete", IsError = true });
            }
            
        }
    }
}
