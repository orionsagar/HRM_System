using Application.Tasks.Commands.CSection;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QSection;
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
using DataEncryption = Utility.DataEncryption;
using Section = Domains.Models.Section;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class SectionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;

        public SectionController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
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
            var CompID = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]));
            var OrgId = _global.GetOrgId();
            var sections = await _mediator.Send(new GetAllSectionQuery() { CompId = CompID, OrgId = OrgId });
            return View(sections);
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
                var empTypeLists = await _mediator.Send(new SP_Dt_SectionListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = comid, OrgId = OrgId });

                //total number of rows counts   
                if (empTypeLists.Count > 0)
                {
                    recordsTotal = empTypeLists.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = empTypeLists });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Somthing Wrong" });
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
            ViewBag.Action = "Add";
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Section section)
        {
            var UserId = _global.GetUserID();
            var CompID =_global.GetCompID();
            var OrgId =_global.GetOrgId();
            if (section.SectId > 0)
            {
                section.UpdatedBy = UserId;
                section.UpdatedDate = DateTime.Now;
                await _mediator.Send(new EditSectionCommand() { Section = section });

                var json = JsonConvert.SerializeObject(section);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = section.SectId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Section", DocumentReferance = json });
            }
            else
            {
                section.AddedBy = UserId;
                section.AddedDate = DateTime.Now;
                section.CompId = CompID;
                await _mediator.Send(new CreateSectionCommand() { Section = section });

                var json = JsonConvert.SerializeObject(section);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = section.SectId.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Section", DocumentReferance = json });
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
            if (id == null)
            {
                return Redirect("/error/404");
            }
            ViewBag.Action = "Edit";
            var section = await _mediator.Send(new GetSectionByIdQuery { SectionId = Convert.ToInt32(id) });

            if (section == null)
            {
                return Redirect("/error/404");
            }
            ViewBag.Action = "Edit";
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);

            return View("Create", section);
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
                var tableData = await _mediator.Send(new GetSectionByIdQuery { SectionId = Convert.ToInt32(Id) });
                if (tableData != null)
                {
                    await _mediator.Send(new DeleteSectionCommand() { SectionId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} Section", DocumentReferance = Id.ToString() });
                }
                return Json(new BLStatus { Message = "Employee Type Delete Successfully", Data = tableData });
            }
            catch (Exception)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable To Delete" });
            }
        }
    }
}
