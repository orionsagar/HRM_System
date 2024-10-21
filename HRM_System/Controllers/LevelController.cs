using Application.Tasks.Queries.QEmployeeType;
using Domains.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using MediatR;
using UKHRM.Helper;
using Application.Tasks.Queries.QEmployee;
using UKHRM.Helpers;
using Application.Tasks.Queries.QLevel;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Commands.CUserInfo;
using Domains.ViewModels;
using Newtonsoft.Json;
using System.IO;
using Utility;
using Microsoft.AspNetCore.Authorization;
using DataEncryption = Utility.DataEncryption;
using Application.Tasks.Commands.CLevel;
using Application.Tasks.Commands.CEmployeeType;

namespace UKHRM.Controllers
{
    public class LevelController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        public LevelController(IMediator mediator = null, IGlobalHelper global = null)
        {
            _mediator = mediator;
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
            var CompID = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]));

            //var OrgID = 0;
            //if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["OrgId"]))
            //{
            //    OrgID = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["OrgId"]));
            //}
            //var emptype = await _mediator.Send(new GetAllQuery() { OrgID = OrgID });
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
                
                var LevelLists = await _mediator.Send(new SP_Dt_LevelListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, OrgId = comid });

                //total number of rows counts   
                if (LevelLists.Count > 0)
                {
                    recordsTotal = LevelLists.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = LevelLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
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
            var ComID = _global.GetCompID();
            //ViewBag.RoleId = await _common.RoleDropdown(ComID);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LevelViewModels level)
        {
            var ComID = _global.GetCompID();
            var UserId = _global.GetUserID();

            if (ModelState.IsValid)
            {
                try
                {
                    level.AddedDate = DateTime.Now;
                    if (level.LevelID > 0)
                    {
                        await _mediator.Send(new UpdateLevelCommand { levelViewModel = level });

                        var json = JsonConvert.SerializeObject(level);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = level.LevelID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Level", DocumentReferance = json });
                    }
                    else
                    {
                        await _mediator.Send(new CreateLevelCommand() { levelViewModels = level });

                        var json = JsonConvert.SerializeObject(level);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = level.LevelID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Level", DocumentReferance = json });
                    }
                    //return Json(new {iserror = false, message = "Register successfully done"});
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception Ex)
                {
                    ///return Json(new { iserror = true, message = Ex.InnerException.Message });
                    ///
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            //return Json(userInfo);
            return View();
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
            var levels = await _mediator.Send(new GetByIdQuery { ID = Convert.ToInt32(id) });

            if (levels == null)
            {
                return Redirect("/error/404");
            }
            return View("Create", levels);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
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
                var tableData = await _mediator.Send(new GetByIdQuery { ID = Convert.ToInt32(Id) });
                if (tableData != null)
                {
                    await _mediator.Send(new DeleteLevelCommand() { Id = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} Level", DocumentReferance = Id.ToString() });
                }
                return Json(new BLStatus { Message = "Delete Successfully", Data = tableData });
            }
            catch (Exception)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable To Delete" });
            }

        }
    }
}
