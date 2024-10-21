using Application.Tasks.Queries.QEmployee;
using AutoMapper;
using UKHRM.Helpers;
using Business.HR;
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
    public class TransferController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITransferBL _TransferBL;
        //private readonly ILogger _logger;

        public TransferController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IMapper mapper, ITransferBL TransferBL)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;

            _mapper = mapper;
            _TransferBL = TransferBL;
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
            var OrgId = _global.GetOrgId();
            //ViewBag.EmpCategory = await _dropdown.EmployeeCategoryDropdown(compId);
            //ViewBag.Designation = await _dropdown.DesignationDropdown(compId);
            //ViewBag.PayScale = await _dropdown.PayscaleDropdown(compId);
            ViewBag.Section = await _dropdown.SectionDropdown(compId, OrgId);
            //ViewBag.Rules = await _dropdown.RulesDropdown("Calculation Rule");
            ViewBag.Department = await _dropdown.DepartmentDropdown(compId, OrgId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(EmployeeFilterVM filter)
        {
            try
            {
                var data = await _mediator.Send(new GetServiceAgeEmployeeQuery { EmployeeFilterVM = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<TransferVM> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {                        
                        item.AddedDate = DateTime.Now;
                        item.AddedBy = _global.GetUserID();
                        item.CompId = _global.GetCompID();
                    }
                    var status = await _TransferBL.Add(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }
        }    
    }
}
