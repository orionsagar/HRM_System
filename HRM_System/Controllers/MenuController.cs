using Application.Tasks.Commands.CMenu;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QMainModule;
using Application.Tasks.Queries.QMenu;
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

namespace UKHRM.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SubModuleController> _logger;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;

        public MenuController(IMediator mediator, ILogger<SubModuleController> logger, CommonDropdown dropdown, IGlobalHelper global)
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
            var compId = _global.GetCompID();
            ViewBag.ModuleId = await _dropdown.ModuleDropdown(compId);
            ViewBag.SubModuleId = await _dropdown.SubModuleDropdown(compId);
            ViewBag.RoleId = await _dropdown.RoleDropdown(compId);
            return View();
        }

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var MdoleId = Convert.ToInt32(Request.Form["moduleId"].FirstOrDefault());
                var subMId = Request.Form["subModuleId"].FirstOrDefault();
                //int? SubModuleId = String.IsNullOrEmpty(subMId) || subMId== "0" ? null : (int.Parse(subMId) as int?);
                int? SubModuleId = String.IsNullOrEmpty(subMId) || subMId== "" ? 0 : (int.Parse(subMId) as int?);

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
                var CompId = _global.GetCompID();

                var menus = await _mediator.Send(new SP_Dt_ModuelMenuListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ModuleId = MdoleId, SubModuleId = SubModuleId, CompId = CompId });

                //total number of rows counts   
                if (menus.Count > 0)
                {
                    recordsTotal = menus.FirstOrDefault().TOTALCOUNT;
                }

                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = menus });
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                _logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
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
            ViewBag.Action = "Create";
            var CompId = _global.GetCompID();
            ViewBag.ModuleId = await _dropdown.ModuleDropdown(CompId);
            ViewBag.SubModuleId = await _dropdown.SubModuleDropdown(CompId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAccessTools accessTools)
        {
            try
            {
                var UserId = _global.GetUserID();
                var RoleId = _global.GetRoleID();
                var CompId = _global.GetCompID();
                var category = accessTools.ModuleID;
                var GetSource = await _mediator.Send(new GetMainModuleByIdQuery { ModuleID = category });
                accessTools.Category = GetSource.ModuleName;
                if (accessTools.UAToolid > 0)
                {
                    await _mediator.Send(new UpdateMenuCommand { AccessTools = accessTools });

                    var json = JsonConvert.SerializeObject(accessTools);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = accessTools.SubModuleID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Menu", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreateMenuCommand() { AccessTools = accessTools });

                    var json = JsonConvert.SerializeObject(accessTools);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = accessTools.SubModuleID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Menu", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
                return View(accessTools);
            }
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
            if (id == null)
            {
                return Redirect("/error/404");
            }
            var CompId = _global.GetCompID();
            ViewBag.ModuleId = await _dropdown.ModuleDropdown(CompId);
            ViewBag.SubModuleId = await _dropdown.SubModuleDropdown(CompId);
            var mdata = await _mediator.Send(new GetMenuByIdQuery { uatoolid = Convert.ToInt32(id) });
            if (mdata == null)
            {
                return Redirect("/error/404");
            }
            return View("Create", mdata);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int? Id)
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
            var tableData = await _mediator.Send(new GetMenuByIdQuery { uatoolid = Convert.ToInt32(Id) });

            if (tableData != null)
            {
                await _mediator.Send(new DeleteMenuCommand() { Id = Convert.ToInt32(Id) });

                var json = JsonConvert.SerializeObject(Id);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} Menu", DocumentReferance = json });
            }
            return Json(data: tableData);
        }
        public async Task<JsonResult> GetMenuSortIndex(int ModuleId, int SubModuleId)
        {
            var lastindex = await _mediator.Send(new GetMenuSortIndexQuery { ModuleId = ModuleId, SubModuleId = SubModuleId });
            var maxindex = lastindex + 1;
            return Json(maxindex);
        }
    }
}
