using Application.Tasks.Queries.QEmployeeStatus;
using AutoMapper;
using UKHRM.Helpers;
using Business.HR;
using Business.Schedule;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class EmployeeStatusController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployeeStatusBL _employeeStatusBL;
        //private readonly ILogger _logger;

        public EmployeeStatusController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IEmployeeStatusBL EmployeeStatusBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _employeeStatusBL = EmployeeStatusBL;
            _mapper = mapper;
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
            var ClientId = _global.GetClientId();
            var orgid = _global.GetOrgId();
            ViewBag.Status =await _dropdown.EmployeeStatusDropdown(compId,orgid);
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(EmpStatusFilter filter)
        {
            try
            {
                var data =await _mediator.Send(new GetEmpByStatusQuery { EmpStatusFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<EmployeeStatusVM> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var compId = _global.GetCompID();
                    var userId =  _global.GetUserID();
                    model.ForEach(a =>
                    {
                        a.CompId = compId;
                        a.UserId = userId;

                   });
                    var status = await _employeeStatusBL.EmployeeHistroyStatusChange(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message=ex.InnerException.Message, StatusCode="500" }) ;
            }           
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EmployeeStatusVM model)
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
                if (ModelState.IsValid)
                {
                    model.CompId = _global.GetCompID();
                    model.UserId =  _global.GetUserID();

                    var status = await _employeeStatusBL.EmployeeHistroyStatusUpdate(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message=ex.InnerException.Message, StatusCode="500" }) ;
            }           
            
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(List<EmployeeStatusVM> model)
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
                var status = await _employeeStatusBL.DeleteStatus(model);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByHistoryId(int empHistoryId)
        {
            try
            {
                if (empHistoryId < 1) return BadRequest();

                var data = await _mediator.Send(new GetEmpStatusByHistoryIdQuery { HistoryId = empHistoryId });
                return Json(new BLStatus { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }


    }
}
