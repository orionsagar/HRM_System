using Application.Tasks.Commands.CEmployeeDesignation;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QEmployeeDesignation;
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
using DataEncryption = Utility.DataEncryption;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class DesignationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;

        public DesignationController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global)
        {
            this._mediator = mediator;
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
            var orgid = _global.GetOrgId();
            var clientid = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientid);
            return View();
        }       

        public async Task<IActionResult> LoadData()
        {
            try
            {
                //var OrgId = Request.Form["OrgId"].FirstOrDefault();
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
                var OrgId = _global.GetOrgId();
                var designations = new List<DesignationVM>();
                var ClientId = _global.GetClientId();
                var RoleType = _global.GetRoleType();
                designations = await _mediator.Send(new GetDesignationListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = ComId, OrgId = Convert.ToInt32(OrgId), ClientId = ClientId, RoleType = RoleType });
                if (designations.Count != 0)
                {
                    if (designations.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = designations.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = designations });
            }
            catch (Exception ex)
            {
                throw ex;
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
            var CompID = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.PayScaleId = await _dropdown.PayscaleDropdown(CompID, OrgId);
            var clientid = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(OrgId, clientid);
            ViewBag.DeptId = await _dropdown.DepartmentDropdown(CompID, OrgId);
            ViewBag.Action = "Add";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Designation designation)
        {
            try
            {
                var orgid = _global.GetOrgId();
                var clientid = _global.GetClientId();
                var UserId = _global.GetUserID();
                var CompID = _global.GetCompID();
                designation.ClientId = clientid;
                designation.OrgId = orgid;
                if (designation.DesigId > 0)
                {
                    
                    designation.UpdatedBy = UserId;
                    designation.UpdatedDate = DateTime.Now;
                    await _mediator.Send(new EditDesignationCommand() { Designation = designation });

                    var json = JsonConvert.SerializeObject(designation);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = designation.DesigId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = "Create Designation", DocumentReferance = json });
                }
                else
                {
                    designation.AddedBy = UserId;
                    designation.AddedDate = DateTime.Now;
                    designation.CompId = CompID;
                    await _mediator.Send(new CreateDesignationCommand() { Designation = designation });

                    var json = JsonConvert.SerializeObject(designation);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = designation.DesigId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Update), TransStatement = $"{Enums.commandtype.Update} Designation", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var CompID = _global.GetCompID();
                var orgid = _global.GetOrgId();
                var clientid = _global.GetClientId();
                ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientid);
                ViewBag.DeptId = await _dropdown.DepartmentDropdown(CompID, orgid);
                return View(designation);
            }
            
        }
        public async Task<IActionResult> Edit(int? id)
        {
            #region Access
            var clientid = _global.GetClientId();
            var roleid = _global.GetRoleID();
            var orgid = _global.GetOrgId();
            var CompID = _global.GetCompID();
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
            //ViewBag.Action = "Edit";
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientid);
            ViewBag.DeptId = await _dropdown.DepartmentDropdown(CompID, orgid);
            var designation = await _mediator.Send(new GetDesignationByIdQuery { DesignationId = Convert.ToInt32(id) });
            
            ViewBag.PayScaleId = await _dropdown.PayscaleDropdown(CompID,orgid);
            if (designation == null)
            {
                return Redirect("/error/404");
            }
            return View("Create", designation);
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
                var tableData = await _mediator.Send(new GetDesignationByIdQuery { DesignationId = Convert.ToInt32(Id) });

                if (tableData != null)
                {
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} Designation", DocumentReferance = Id.ToString() });

                    await _mediator.Send(new DeleteDesignationCommand() { DesignationId = Convert.ToInt32(Id) });
                }
                return Json(new BLStatus { IsError = false, Message = "Designation Delete Successfully" , Data = tableData});
            }
            catch (Exception)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
            
        }
    }
}
