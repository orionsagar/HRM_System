using Application.Tasks.Commands.CEmployeeType;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QEmployee;
using Application.Tasks.Queries.QEmployeeType;
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

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class EmployeeTypeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly Helper.CommonDropdown _dropdown;

        public EmployeeTypeController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
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
            var CompID = Convert.ToInt32(DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]));
            var OrgId = _global.GetOrgId();
            var emptype = await _mediator.Send(new GetAllEmployeeTyeQuery() { CompId = CompID, OrgId = OrgId });
            return View(emptype);
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
                //var comid = _global.GetCompID();
                var orgid = _global.GetOrgId();
                var empTypeLists = await _mediator.Send(new SP_Dt_EmpTypeListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search,OrgId = orgid });

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
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            ViewBag.Action = "Add";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeType employeeType)
        {
            var UserId = _global.GetUserID();
            var CompID = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            employeeType.OrgId = OrgId;
            if(employeeType.EmpTypeId > 0)
            {
                employeeType.UpdatedBy = UserId;
                employeeType.UpdatedDate = DateTime.Now;
                await _mediator.Send(new EditEmployeeTypeCommand() { EmployeeType = employeeType });

                var json = JsonConvert.SerializeObject(employeeType, Formatting.Indented);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = employeeType.EmpTypeId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = $"{Enums.commandtype.Create} EmployeeType", DocumentReferance = json });
            }
            else
            {
                employeeType.AddedBy = UserId;
                employeeType.AddedDate = DateTime.Now;
                employeeType.CompId = CompID;
                await _mediator.Send(new CreateEmployeeTypeCommand() { EmployeeType = employeeType });

                var json = JsonConvert.SerializeObject(employeeType, Formatting.Indented);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = employeeType.EmpTypeId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Update), TransStatement = $"{Enums.commandtype.Update} EmployeeType", DocumentReferance = json });
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
            var employeeType = await _mediator.Send(new GetEmployeeTypeByIdQuery { EmpTypeId = Convert.ToInt32(id) });
            
            if (employeeType == null)
            {
                return Redirect("/error/404");
            }
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);

            return View("Create", employeeType);
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
                var tableData = await _mediator.Send(new GetEmployeeTypeByIdQuery { EmpTypeId = Convert.ToInt32(Id) });
                if (tableData != null)
                {
                    await _mediator.Send(new DeleteEmployeeTypeCommand() { EmpTypeId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} EmployeeType", DocumentReferance = Id.ToString() });
                }
                return Json(new BLStatus { Message = "Employee Type Delete Successfully",Data = tableData });
            }
            catch (Exception)
            {
                return Json(new BLStatus { IsError = true,Message = "Unable To Delete" });
            }
            
        }
    }
}
