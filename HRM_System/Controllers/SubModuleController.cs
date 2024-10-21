using Application.Tasks.Commands.CSubModule;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QMainModule;
using Application.Tasks.Queries.QSubModule;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
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
    public class SubModuleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly ILogger<SubModuleController> _logger;
        private readonly CommonDropdown _dropdown;

        public SubModuleController(IMediator mediator, ILogger<SubModuleController> logger, CommonDropdown dropdown, IGlobalHelper global)
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
                //Paging Size (10, 20, 50,100)  
                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var comid = _global.GetCompID();
                var subModules = await _mediator.Send(new SP_Dt_SubModuelListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, ModuleId = moduleid,ComId = comid });
               
                //total number of rows counts   
                if(subModules.Count > 0)
                {
                    recordsTotal = subModules.FirstOrDefault().TOTALCOUNT;
                }
                //Paging   
                //var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = subModules });
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
            var CompId = _global.GetCompID();
            ViewBag.ModuleId = await _dropdown.ModuleDropdown(CompId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubModule subModule)
        {
            try
            {
                if (subModule.SubModuleID != 0)
                {
                    await _mediator.Send(new UpdateSubModuleCommand { SubModule = subModule });

                    var json = JsonConvert.SerializeObject(subModule);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = subModule.SubModuleID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} SubModule", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreateSubModuleCommand() { SubModule = subModule });

                    var json = JsonConvert.SerializeObject(subModule);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = subModule.SubModuleID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} SubModule", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
                return View(subModule);
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
            var mdata = await _mediator.Send(new GetSubModuleByIdQuery { SubModuleId = Convert.ToInt32(id) });
            if (mdata == null)
            {
                return Redirect("/error/404");
            }
            return View("Create",mdata);
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
            var tableData = await _mediator.Send(new GetSubModuleByIdQuery { SubModuleId = Convert.ToInt32(Id) });

            if (tableData != null)
            {
                await _mediator.Send(new DeleteSubModuleCommand() { SubModuleId = Convert.ToInt32(Id) });

                var json = JsonConvert.SerializeObject(Id);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} SubModule", DocumentReferance = json });
            }
            return Json(data: tableData);
        }
        public async Task<JsonResult> GetSortIndexByModuleId(int ModuleId)
        {
            var lastindex = await _mediator.Send(new GetSortIndexByModuleIdQuery { ModuleId = ModuleId });
            var maxindex = lastindex + 1;
            return Json(maxindex);
        }

        public async Task<JsonResult> GetSubModuleByModuleId(int ModuleId)
        {
            var subModules = await _mediator.Send(new GetSubModuleByModuleIdQuery { ModuleId = ModuleId });
            return Json(subModules);
        }

    }
}
