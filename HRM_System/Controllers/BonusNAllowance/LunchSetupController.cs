using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Application.Tasks.Commands.CLunchSetup;
using Application.Tasks.Queries.QLunchSetup;
using UKHRM.Helpers;
using Business.BonusNAllowance;
using Domains.Models;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class LunchSetupController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly ILunchSetupBL _lunchSetupBL;

        public LunchSetupController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global, ILunchSetupBL lunchSetupBL)
        {
            _mediator = mediator;
            _dropdown = dropdown;
            _global = global;
            _lunchSetupBL = lunchSetupBL;
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
            ViewBag.Action = "Add";
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.LunchSetupList = await _mediator.Send(new GetAllQuery() { OrgId= OrgId });
            ViewBag.Designation = await _dropdown.DesignationDropdown(ComId, OrgId);
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SnacksAllowance model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var orgid = _global.GetOrgId();
                    model.OrgId = orgid;
                    if (await _lunchSetupBL.CheckExistByDesignation(model))
                    {
                        ViewBag.Designation = await _dropdown.DesignationDropdown(model.CompId, model.DesigId);
                        ModelState.AddModelError(String.Empty, "The designation already have a lunch setup!!");
                        return View(nameof(Index), model);
                    }
                    else
                    {
                        await _lunchSetupBL.Upsert(model);
                        return RedirectToAction(nameof(Index));
                    }
                    //if (model.SnacksAllowanceID != 0)
                    //{
                    //    await _mediator.Send(new UpdateLunchSetupCommand() { SnacksAllowance = model });
                    //}
                    //else
                    //{
                    //    await _mediator.Send(new CreateLunchSetupCommand() { SnacksAllowance = model });
                    //}
                    //return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Designation = await _dropdown.DesignationDropdown(model.CompId, model.DesigId);
                    ModelState.AddModelError(String.Empty, "Submited data is not valid!!");
                    return View(nameof(Index), model);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Something Wrong";
                return View(nameof(Index), model);
                throw;
            }
        }



        public async Task<IActionResult> Edit(int id)
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
            var ComId = _global.GetCompID();
            ViewBag.Action = "Edit";
            var lunch = await _mediator.Send(new GetByIdQuery() { SnacksId = id });
            ViewBag.LunchSetupList = await _mediator.Send(new GetAllQuery() { ComId = ComId });
            ViewBag.Designation = await _dropdown.DesignationDropdown(ComId, lunch.DesigId);
            return View("Index", lunch);
        }
        //[HttpPost]
        public async Task<IActionResult> Delete(int id)
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
                var data = await _mediator.Send(new GetByIdQuery() { SnacksId = id });
                if (data != null)
                {
                    await _mediator.Send(new DeleteLunchSetupCommand() { SancksId = id });
                }
                return Json(new BLStatus { Message = "Lunch Setup Delete Successfully", IsError = false });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { Message = "Lunch Setup Already Assign", IsError = true });
            }

        }


    }
}
