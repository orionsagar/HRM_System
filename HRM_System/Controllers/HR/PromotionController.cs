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
    public class PromotionController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPromotionBL _promotionBL;
        //private readonly ILogger _logger;

        public PromotionController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IMapper mapper, IPromotionBL promotionBL)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;

            _mapper = mapper;
            _promotionBL = promotionBL;
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
            ViewBag.Designation = await _dropdown.DesignationDropdown(compId,OrgId);
            ViewBag.PayScale = await _dropdown.PayscaleDropdown(compId, OrgId);
            //ViewBag.Section = await _dropdown.SectionDropdown(compId);
            //ViewBag.Rules = await _dropdown.RulesDropdown("Calculation Rule");
            //ViewBag.Department = await _dropdown.DepartmentDropdown(compId);
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
        public async Task<IActionResult> Create(List<PromotionVM> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        item.DecisionDate = DateTime.Now;
                        item.AddedDate = DateTime.Now;
                        item.AddedBy = _global.GetUserID();
                        item.CompId = _global.GetCompID();
                    }

                    var status = await _promotionBL.Add(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
            }

        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Halt(Promotion model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (model.PromotionID > 0)
        //            {
        //                model.UpdatedBy = _global.GetUserID();
        //                model.UpdatedDate = DateTime.Now;
        //            }
        //            else
        //            {
        //                model.AddedBy = _global.GetUserID();
        //                model.AddedDate = DateTime.Now;
        //            }
        //            model.CompId = _global.GetCompID();

        //            var status = await _PromotionBL.Halt(model);
        //            return Json(status);
        //        }

        //        return Json(new BLStatus { IsError = true, Message = "Submited data not valid.", StatusCode = "422" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new BLStatus { IsError = true, Message = ex.InnerException.Message, StatusCode = "500" });
        //    }

        //}





        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> DeleteByEmpId(List<int> empIds)
        ////{
        ////    try
        ////    {
        ////        var status = await _advanceSanctionBL.DeleteByEmpId(empIds);
        ////        return Json(status);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
        ////    }
        ////}

        //[HttpDelete]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int PromotionId)
        //{
        //    try
        //    {
        //        var status = await _PromotionBL.Delete(PromotionId);
        //        return Json(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
        //    }
        //}



        //[HttpGet]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> GetByAdvanceId(int advanceId)
        //{
        //    try
        //    {
        //        if (advanceId < 1) return BadRequest();

        //        var data = await _mediator.Send(new GetPromotionByAdvanceIdQuery { AdvanceId = advanceId });
        //        return Json(new BLStatus { Data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
        //    }
        //}

        //[HttpGet]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> GetById(int PromotionId)
        //{
        //    try
        //    {
        //        if (PromotionId < 1) return BadRequest();

        //        var data = await _mediator.Send(new GetPromotionByIdQuery { PromotionId = PromotionId });
        //        return Json(new BLStatus { Data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
        //    }
        //}


    }
}
