using Application.Tasks.Commands.CEmployeeCategory;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QEmployeeCategory;
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
    public class EmployeeCategoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;

        public EmployeeCategoryController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
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
            //var CompID = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]));
            //var sections = await _mediator.Send(new GetAllEmployeeCategoryQuery() { CompId = CompID });
            var orgid = _global.GetOrgId();
            var clientId = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid,clientId);

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
                var clientId = _global.GetClientId();
                var roletype = _global.GetRoleType();
                var orgid = _global.GetOrgId();
                var categors = await _mediator.Send(new SP_Dt_CategoryListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = comid, ClientId = clientId, OrgId = orgid, RoleType = roletype });

                //total number of rows counts   
                if (categors.Count > 0)
                {
                    recordsTotal = categors.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = categors });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public async Task<IActionResult> Create(EmployeeCategory employeeCategory)
        {
            var UserId = _global.GetUserID();
            var CompID = _global.GetCompID();
            var ClientId = _global.GetClientId();
            if (employeeCategory.EmpCatId > 0)
            {
                employeeCategory.UpdatedBy = UserId;
                employeeCategory.UpdatedDate = DateTime.Now;
                await _mediator.Send(new EditEmployeeCategoryCommand() { EmployeeCategory = employeeCategory });

                var json = JsonConvert.SerializeObject(employeeCategory);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = employeeCategory.EmpCatId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Update), TransStatement = $"{Enums.commandtype.Update} Category", DocumentReferance = json });
            }
            else
            {
                
                employeeCategory.AddedBy = UserId;
                employeeCategory.AddedDate = DateTime.Now;
                employeeCategory.CompId = CompID;
                employeeCategory.ClientId = ClientId;
                await _mediator.Send(new CreateEmployeeCategoryCommand() { EmployeeCategory  = employeeCategory });

                var json = JsonConvert.SerializeObject(employeeCategory);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = employeeCategory.EmpCatId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = $"{Enums.commandtype.Create} Category", DocumentReferance = json });
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
            var section = await _mediator.Send(new GetEmployeeCategoryByIdQuery { EmpCatId = Convert.ToInt32(id) });

            if (section == null)
            {
                return Redirect("/error/404");
            }
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
                var tableData = await _mediator.Send(new GetEmployeeCategoryByIdQuery { EmpCatId = Convert.ToInt32(Id) });

                if (tableData != null)
                {                    
                    await _mediator.Send(new DeleteEmployeeCategoryCommand() { EmpCatId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} Category", DocumentReferance = Id.ToString() });
                }
                return Json(new BLStatus { Message = "Category Delete Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable To Delete" });
            }
            
        }
    }
}
