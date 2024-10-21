using Application.SupportiveBL.Context;
using Application.Tasks.Commands.CClient;
using Application.Tasks.Commands.COrganisation;
using Application.Tasks.Handlers.HCompany;
using Application.Tasks.Queries.QClient;
using Application.Tasks.Queries.QOrganisation;
using Application.Tasks.Queries.QPayScale;
using UKHRM.Helpers;
using Application.Tasks.Queries.QUserInfo;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UKHRM.Helper;
using Application.Tasks.Commands.CTransactionLog;
using Newtonsoft.Json;
using static Domains.Models.Recruitment;
using Utility;
using Newtonsoft.Json.Linq;

namespace UKHRM.Controllers
{
    public class OrganisationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;
        private readonly IAuthorizationContext _authContext;
        public OrganisationController(IMediator mediator, IGlobalHelper global, CommonDropdown dropdown, IAuthorizationContext authContext)
        {
            _mediator = mediator;
            _global = global;
            _dropdown = dropdown;
            _authContext = authContext;
            _authContext.UserId = _global.GetUserID();
            _authContext.RoleId = _global.GetRoleID();
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
            return View();
        }


        /// <summary>
        /// Organization Profile 
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OrganisationProfile()
        {
            //Adding functionality of the page here
            return View();
        }




        public async Task<IActionResult> LoadData()
        {
            try
            {
                //var dtFrom = Request.Form["dtFrom"].FirstOrDefault();
                //var dtTo = Request.Form["dtTo"].FirstOrDefault();

                var draw = Request.Form["draw"].FirstOrDefault();
                // number of Rows count 
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Index  
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var search = Request.Form["search[value]"].FirstOrDefault();
                var totalrecord = 0;
                // Cookies value get
                var UserId = _global.GetUserID();
                var ClientId = _global.GetClientId();
                var orgid = _global.GetOrgId();
                var RoleType = _global.GetRoleType();
                // Object Diclaration
                var organisations = new List<OrganisationVM>();
                var roletype = _global.GetRoleType();

                organisations = await _mediator.Send(new SP_Dt_OrganisationListQuery() { DisplayLength = Convert.ToInt32(length), Start = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ClientId = ClientId, OrgId = orgid, RoleType = RoleType });

                if (organisations.Count != 0)
                {
                    if (organisations.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = organisations.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = organisations });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetTopFiveOrg()
        {
            var ClientId = _global.GetClientId();
            var orglist = await _global.GetTopFiveOrg(ClientId);
            return Json(new { draw = 1, recordsFiltered = 5, recordsTotal = 5, data = orglist });
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Action = "Add";
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.Designation = await _dropdown.DesignationDropdown(ComId, OrgId);
            var clientid = _global.GetClientId();
            ViewBag.Client = await _dropdown.ClientDropdown(clientid);
            ViewBag.Country = await _dropdown.CountryDropdown();
            ViewBag.PackageId = await _dropdown.PackageDropdown();
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganisationVM organisation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    organisation.AddedBy = _global.GetUserID();
                    if (organisation.ClientId == 0)
                    {
                        organisation.ClientId = _global.GetClientId();
                    }
                    var orgid = await _mediator.Send(new OrganisationCommand { OrganisationVM = organisation });
                    //============ Log Entry ==============//
                    var json = JsonConvert.SerializeObject(organisation);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = orgid.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = $"{Enums.commandtype.Create} Organisation", DocumentReferance = json });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                    var OrgId = _global.GetOrgId();
                    ViewBag.Designation = await _dropdown.DesignationDropdown(1, OrgId);
                    ViewBag.Client = await _dropdown.ClientDropdown();
                    ViewBag.Country = await _dropdown.CountryDropdown();
                    ViewBag.PackageId = await _dropdown.PackageDropdown();
                    if (organisation.OrgId > 0)
                    {
                        return RedirectToAction(nameof(Edit), organisation.OrgId);
                    }
                    return View(organisation);
                    //return Json(new BLStatus { IsError = true, Message = $"Unable To Saved. error is {ModelState.Values}", StatusCode = "500" });
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
                //return Json(new BLStatus { IsError = true, Message = $"Unable To Saved. error is {ex.Message}", StatusCode = "500" });
            }

        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Action = "Edit";
            var ClientId = _global.GetClientId();
            var OrgId = _global.GetOrgId();
            if (id == null)
            {
                return Redirect("/error/404");
            }
            var orgdata = new OrganisationVM();
            orgdata = await _mediator.Send(new GetOrganisationDataByIdQuery { OrgId = Convert.ToInt32(id) });
            var user = await _mediator.Send(new GetUserBy_OrgId_RoleId_ClientId_Query { OrgId = Convert.ToInt32(id) });
            if (user != null)
            {
                orgdata.SA_FirstName = user.FirstName;
                orgdata.SA_LastName = user.LastName;
                orgdata.SA_Email = user.Email;
            }

            if (orgdata == null)
            {
                return Redirect("/error/404");
            }
            ViewBag.Designation = await _dropdown.DesignationDropdown(1, OrgId);
            ViewBag.Country = await _dropdown.CountryDropdown();
            ViewBag.Client = await _dropdown.ClientDropdown(ClientId);
            ViewBag.PackageId = await _dropdown.PackageDropdown();
            return View("Create", orgdata);
        }

        public async Task<IActionResult> GetOrganisationById(int OrgId)
        {
            var orgdata = await _mediator.Send(new GetOrganisationDataByIdQuery { OrgId = OrgId });
            return Json(orgdata);
        }

        [HttpPost]
        public IActionResult _OrgDetails(OrganisationVM data)
        {
            return PartialView("_OrgDetails", data);
        }
        public async Task<IActionResult> CheckOrgName(string OrgName)
        {
            var name = await _global.CheckOrgName(OrgName);
            return Json(name);
        }


        public async Task<IActionResult> OrgProfile(int? id)
        {
            var orgid = _global.GetOrgId();
            var userid = UKHRM.Helper.DataEncryption.DecryptString(HttpContext.Request.Cookies["UserID"]);
            var userinfo = await _mediator.Send(new GetUserInfoByIdQuery() { UserID = Convert.ToInt32(userid) });

            var orgdata = new OrganisationVM();
            orgdata = await _mediator.Send(new GetOrganisationDataByIdQuery { OrgId = Convert.ToInt32(id) });
            var user = await _mediator.Send(new GetUserBy_OrgId_RoleId_ClientId_Query { OrgId = Convert.ToInt32(id) });
            if (user != null)
            {
                orgdata.SA_FirstName = user.FirstName;
                orgdata.SA_LastName = user.LastName;
                orgdata.SA_Email = user.Email;
            }
            return View(orgdata);
        }
    }
}
