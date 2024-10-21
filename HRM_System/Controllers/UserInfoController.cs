using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Commands.CUserInfo;
using Application.Tasks.Queries.QUserInfo;
using AutoMapper;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using DataEncryption = Utility.DataEncryption;
using Business;
using Application.Tasks.Queries.QEmployeeCategory;
using Persistence.Repository;
using Persistence.Repository.Clients;

namespace UKHRM.Controllers
{
    //[Authorize]
    public class UserInfoController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserInfoController> _logger;
        private readonly CommonDropdown _common;
        private readonly IMapper _mapper;
        private readonly IGlobalHelper _global;
        private readonly IAuthBL _auth;
        private readonly IClientProfile _clientProfile;

        public UserInfoController(IMediator mediator, ILogger<UserInfoController> logger, CommonDropdown common, IMapper mapper, IGlobalHelper global, IAuthBL auth, IClientProfile clientProfile)
        {
            _mediator = mediator;
            _logger = logger;
            _common = common;
            _mapper = mapper;
            _global = global;
            _auth = auth;
            _clientProfile = clientProfile;
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var RoleName = _global.GetRoleName();
            var RoleID = Convert.ToInt32(_global.GetRoleID());
            var ClientID = Convert.ToInt32(_global.GetClientId());
            var UserID = Convert.ToInt32(_global.GetUserID());

            if (UserID <= 0)
            {
                return BadRequest("Invalid User ID.");
            }
            //var userProfile = await _clientProfile.GetClientProfile(UserID, RoleName, RoleID);
            var userProfile = await _clientProfile.GetUserProfile(UserID);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return View(userProfile);
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
            var clientId = _global.GetClientId();
            ViewBag.OrgId = await _common.OrganisationDropdown(orgid, clientId);
            var RoleType = DataEncryption.DecryptString(HttpContext.Request.Cookies["RoleType"]);
            if (RoleType != null)
            {
                if (RoleType == "Org_Role")
                {
                    ViewBag.RoleType = "HideLink";
                }
                else
                {
                    ViewBag.RoleType = "ShowLink";
                }
            }

            return View();
        }



        [AllowAnonymous]
        public async Task<IActionResult> LoadData()
        {
            try
            {
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
                var OrgId = _global.GetOrgId();
                var userlist = await _mediator.Send(new SP_Dt_UserListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = comid, ClientId = clientId, OrgId = OrgId, RolelType = roletype });

                //total number of rows counts   
                if (userlist.Count > 0)
                {
                    recordsTotal = userlist.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = userlist });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
                throw;
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
            ViewBag.Action = "Create";
            var ComID = _global.GetCompID();
            var RoleType = _global.GetRoleType();
            ViewBag.RoleId = await _common.RoleDropdown(ComID, 0, RoleType);
            var orgid = _global.GetOrgId();
            var clientid = _global.GetClientId();
            ViewBag.OrgId = await _common.OrganisationDropdown(orgid, clientid);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserInfoViewModel userInfo)
        {
            var ComID = _global.GetCompID();
            var UserId = _global.GetUserID();
            var ClientId = _global.GetClientId();
            var OrgId = _global.GetOrgId();
            var RoleId = await _auth.GET_RoleID_BY_U_ROLENAME();
            if (ModelState.IsValid)
            {
                try
                {
                    userInfo.RoleID = userInfo.RoleID > 0 ? userInfo.RoleID : RoleId;
                    userInfo.Email = userInfo.Email;
                    userInfo.AddedDate = DateTime.Now;
                    userInfo.ClientId = ClientId;
                    if (userInfo.UserID > 0)
                    {
                        await _mediator.Send(new UpdateUserInfoCommand { userInfoViewModel = userInfo });

                        var json = JsonConvert.SerializeObject(userInfo);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = userInfo.UserID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} User", DocumentReferance = json });
                    }
                    else
                    {
                        userInfo.OrgId = OrgId;
                        await _mediator.Send(new CreateUserInfoCommand() { userInfoViewModel = userInfo });

                        var json = JsonConvert.SerializeObject(userInfo);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = userInfo.UserID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} User", DocumentReferance = json });
                    }
                    //return Json(new {iserror = false, message = "Register successfully done"});
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception Ex)
                {
                    ///return Json(new { iserror = true, message = Ex.InnerException.Message });
                    ///
                    return View();
                }
            }
            else
            {

                ViewBag.RoleId = await _common.RoleDropdown(ComID);
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            //return Json(userInfo);
            return View();
        }

        public async Task<IActionResult> Create1(UserInfoViewModel userInfo)
        {
            var ComID = _global.GetCompID();
            var UserId = _global.GetUserID();
            var ClientId = _global.GetClientId();
            if (ModelState.IsValid)
            {
                try
                {
                    if (userInfo.UserID > 0)
                    {
                        userInfo.ClientId = userInfo.ClientId == 0 ? ClientId : userInfo.ClientId;
                        await _mediator.Send(new UpdateUserInfoCommand { userInfoViewModel = userInfo });

                        var json = JsonConvert.SerializeObject(userInfo);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = userInfo.UserID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} User", DocumentReferance = json });
                    }
                    else
                    {
                        await _mediator.Send(new CreateUserInfoCommand() { userInfoViewModel = userInfo });

                        var json = JsonConvert.SerializeObject(userInfo);

                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = userInfo.UserID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} User", DocumentReferance = json });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
            else
            {

                ViewBag.RoleId = await _common.RoleDropdown(ComID);
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(userInfo);
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
            var ComID = _global.GetCompID();
            if (id == 0)
            {
                return Redirect("/error/404");
            }
            var tableData = await _mediator.Send(new GetUserInfoByIdQuery { UserID = Convert.ToInt32(id) });
            if (tableData == null)
            {
                return Redirect("/error/404");
            }
            var roletype = _global.GetRoleType();
            ViewBag.RoleId = await _common.RoleDropdown(ComID, 0, roletype);
            ViewBag.UserRole = _global.GetRoleName();
            var orgid = _global.GetOrgId();
            ViewBag.OrgId = await _common.OrganisationDropdown(orgid);
            return View("Create", tableData);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int? Id)
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
            var tableData = await _mediator.Send(new GetUserInfoByIdQuery { UserID = Convert.ToInt32(Id) });

            if (tableData != null)
            {
                await _mediator.Send(new DeleteUserInfoCommand() { Id = Convert.ToInt32(Id) });

                var json = JsonConvert.SerializeObject(Id);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} User", DocumentReferance = json });
            }
            return Json(data: tableData);
        }

        public async Task<IActionResult> Preview()
        {
            try
            {
                var userData = await _mediator.Send(new GetAllUserInfoQuery());
                var ComID = DataEncryption.DecryptString(HttpContext.Request.Cookies["CompID"]);
                //var companys = await _mediator.Send(new GetCompanyByIdQuery { CompID = Convert.ToInt32(ComID) });
                //ViewBag.Company = _mapper.Map<CompanyViewModel>(companys);
                return View(userData);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<IActionResult> GetUserDataByEmail(string Email)
        {
            var userdata = await _mediator.Send(new GetUserDataByEmailQuery { Email = Email });
            return Json(userdata);
        }
    }
}
