using Application.Tasks.Commands.CDepartment;
using Application.Tasks.Commands.CTransactionLog;
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
using static Dapper.SqlMapper;
using Utility;
using Domains.ViewModels;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;

        public DepartmentController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown)
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
            var orgid = _global.GetOrgId();
            var clientId = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientId);

            return View();
        }
        public async Task<IActionResult> LoadData()
        {
            try
            {
                //var OrgId = Request.Form["OrgId"].FirstOrDefault();
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
                var clientid = _global.GetClientId();
                var roletype = _global.GetRoleType();
                var OrgId = _global.GetOrgId();
                var param = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    CompId = Convert.ToInt32(comid),
                    OrgId = OrgId,
                    ClientId = clientid,
                    RoleType = roletype
                };
                var payScaleTypes = await _mediator.Send(new SP_Dt_DepartmentListQuery() { param = param });

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
                //_logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
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
        public async Task<IActionResult> Create(Department department)
        {
            department.ClientId = _global.GetClientId();
            if (department.DeptId > 0)
            {
                await _mediator.Send(new EditDepartmentCommand() { Department = department });

                var json = JsonConvert.SerializeObject(department);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = department.DeptId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Update), TransStatement = $"{Enums.commandtype.Update} Department", DocumentReferance = json });
            }
            else
            {                
                await _mediator.Send(new CreateDepartmentCommand() { Department = department });

                var json = JsonConvert.SerializeObject(department);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = department.DeptId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = "Add Department", DocumentReferance = json });
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
            var section = await _mediator.Send(new GetDepartmentByIdQuery { DepartmentId = Convert.ToInt32(id) });

            if (section == null)
            {
                return Redirect("/error/404");
            }
            var orgid = _global.GetOrgId();
            var clientId = _global.GetClientId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, clientId);

            return View("Create", section);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                var tableData = await _mediator.Send(new GetDepartmentByIdQuery { DepartmentId = Convert.ToInt32(Id) });

                if (tableData != null)
                {
                    await _mediator.Send(new DeleteDepartmentCommand() { DepartmentId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = "Delete Department", DocumentReferance = Id.ToString() });
                }
                return Json(new BLStatus { Data = tableData, Message = "Data Successfully Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }
    }
}
