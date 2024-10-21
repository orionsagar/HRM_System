using Application.Tasks.Commands.CMainModule;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Commands.CUserRole;
using Application.Tasks.Queries.QMainModule;
using Application.Tasks.Queries.QSubModule;
using Application.Tasks.Queries.QUserRole;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers
{
    [Authorize]
    public class UserRoleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly CommonDropdown _dropdown;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(IMediator mediator, ILogger<UserRoleController> logger, IGlobalHelper global, CommonDropdown dropdown)
        {
            _mediator = mediator;
            _logger = logger;
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
            ViewBag.ReportTypeId = await _dropdown.ReportTypeDropdown();            
            return View();
        }

        [HttpPost]
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = await _mediator.Send(new GetAllUserRoleQuery());
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(x => x.RoleName == sortColumn + " " + sortColumnDirection).ToList();
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = (List<UserRole>)customerData.Where(m => m.RoleName != null && m.RoleName.ToUpper().Contains(searchValue.ToUpper())).ToList();
                }
                //total number of rows counts   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                _logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
                throw;
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
            ViewBag.Action = "Create";
            ViewBag.OrgId = await _dropdown.OrganisationDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                if(role.RoleID > 0)
                {
                    await _mediator.Send(new UpdateUserRoleCommand { UserRoleViewModel = role });

                    var json = JsonConvert.SerializeObject(role);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = role.RoleID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Role", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreateUserRoleCommand() { UserRoleViewModel = role });

                    var json = JsonConvert.SerializeObject(role);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = role.RoleID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Role", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(role);
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
            ViewBag.Action = "Edit";
            if (id == null)
            {
                return Redirect("/error/404");
            }
            var tableData = await _mediator.Send(new GetUserRoleByIdQuery { RoleID = Convert.ToInt32(id) });
            if (tableData == null)
            {
                return Redirect("/error/404");
            }
            return View("Create",tableData);
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
            var tableData = await _mediator.Send(new GetUserRoleByIdQuery { RoleID = Convert.ToInt32(Id) });

            if (tableData != null)
            {
                await _mediator.Send(new DeleteUserRoleCommand() { Id = Convert.ToInt32(Id) });
                var json = JsonConvert.SerializeObject(Id);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Delete} Role", DocumentReferance = json });
            }
            return Json(data: tableData);
        }
        
        public async Task<IActionResult> ModuleAccess(int? Id)
        {
            var UserModule = await _mediator.Send(new GetAllUserRoleWiseModuleQuery() { RoleID = Convert.ToInt32(Id) });
            List<SelectListItemModel> items = await PopulateModule();

            for (var i = 0; i < UserModule.Count(); i++)
            {
                if (Convert.ToInt32(items[i].Value) == UserModule[i].ModuleID)
                {
                    items[i].Selected = true;
                }
            }

            UserRoleModuleIViewModel model = new UserRoleModuleIViewModel();
            model.RoleID = Convert.ToInt32(Id);
            model.selectListItems = items;
            return PartialView("_ModuleAccess", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModuleAccess(UserRoleModuleIViewModel userRoleModuleIViewModel)
        {
            List<UserRolewiseModule> UserRolewiseModule = new List<UserRolewiseModule>();
            var RoleID = userRoleModuleIViewModel.RoleID;
            // Split the `rowOrder` which is the array of `unique index`
            string[] uniqueIndexes = Request.Form["tblAppendGrid_rowOrder"].ToString().Split(',');

            List<UserAccessList> UserAccessList = new List<UserAccessList>();
            // Process on each row by using for-loop
            for (int row = 0; row < uniqueIndexes.Length; row++)
            {
                // Get the posted values by using `grid ID` + `column name` + `unique index` syntax
                string ModuleID = Request.Form["tblAppendGrid_moduleID_" + uniqueIndexes[row]];
                string moduleName = Request.Form["tblAppendGrid_moduleName_" + uniqueIndexes[row]];
                string description = Request.Form["tblAppendGrid_description_" + uniqueIndexes[row]];
                string isStatus = Request.Form["tblAppendGrid_isStatus_" + uniqueIndexes[row]];

                // Do whatever to fit your needs, such as save to database
                if (isStatus == "1")
                {
                    UserRolewiseModule.Add(new UserRolewiseModule
                    {
                        ModuleID = Convert.ToInt32(ModuleID),
                        RoleID = Convert.ToInt32(userRoleModuleIViewModel.RoleID),
                        AddedDate = DateTime.Now,
                        AddedBy = _global.GetUserID()
                    });
                }

            }

            if (userRoleModuleIViewModel.selectListItems != null)
            {
                await _mediator.Send(new CreateUserRoleWiseModuleCommand() { RoleId = userRoleModuleIViewModel.RoleID, UserRolewiseModule = UserRolewiseModule });
            }
            ViewBag.Msg = "Update Successfully";

            List<SelectListItemModel> items = await PopulateModule();
            UserRoleModuleIViewModel model = new UserRoleModuleIViewModel();
            model.RoleID = Convert.ToInt32(userRoleModuleIViewModel.RoleID);
            model.selectListItems = items;

            return PartialView("_ModuleAccess", model);
        }

        public async Task<IActionResult> SubModuleAccess(int? Id)
        {
            var CompId = _global.GetCompID();
            var UserModule = await _mediator.Send(new GetAllUserRoleWiseSubModuleQuery() { RoleId = Convert.ToInt32(Id) });
            List<SelectListItemModel> items = await _mediator.Send(new GetSubModuleDropdownQuery() { CompID = CompId });

            for (var i = 0; i < UserModule.Count(); i++)
            {
                if (Convert.ToInt32(items[i].Value) == UserModule[i].ModuleID)
                {
                    items[i].Selected = true;
                }
            }

            UserRoleSubModuleIVM model = new UserRoleSubModuleIVM();
            model.RoleID = Convert.ToInt32(Id);
            model.selectListItems = items;
            return PartialView("_SubModuleAccess", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubModuleAccess(UserRoleSubModuleIVM userRoleSubModuleIVM)
        {
            List<UserRolewiseSubModule> userRolewiseSubModule = new List<UserRolewiseSubModule>();
            var RoleID = userRoleSubModuleIVM.RoleID;
            // Split the `rowOrder` which is the array of `unique index`
            string[] uniqueIndexes = Request.Form["tblAppendGrid_rowOrder"].ToString().Split(',');

            List<UserAccessList> UserAccessList = new List<UserAccessList>();
            // Process on each row by using for-loop
            for (int row = 0; row < uniqueIndexes.Length; row++)
            {
                // Get the posted values by using `grid ID` + `column name` + `unique index` syntax
                string ModuleID = Request.Form["tblAppendGrid_moduleID_" + uniqueIndexes[row]];
                string SubModuleID = Request.Form["tblAppendGrid_subModuleID_" + uniqueIndexes[row]];
                string SubModuleName = Request.Form["tblAppendGrid_subModuleName_" + uniqueIndexes[row]];
                string moduleName = Request.Form["tblAppendGrid_moduleName_" + uniqueIndexes[row]];
                string description = Request.Form["tblAppendGrid_description_" + uniqueIndexes[row]];
                string isStatus = Request.Form["tblAppendGrid_isStatus_" + uniqueIndexes[row]];

                // Do whatever to fit your needs, such as save to database
                if (isStatus == "1")
                {
                    userRolewiseSubModule.Add(new UserRolewiseSubModule
                    {
                        ModuleID = Convert.ToInt32(ModuleID),
                        RoleID = Convert.ToInt32(userRoleSubModuleIVM.RoleID),
                        SubModuleID = Convert.ToInt32(SubModuleID),
                        AddedBy = userRoleSubModuleIVM.AddedBy,
                        AddedDate = userRoleSubModuleIVM.AddedDate
                    });
                }

            }

            if (userRoleSubModuleIVM.selectListItems != null)
            {
                await _mediator.Send(new CreateUserRoleWiseSubModuleCommand() { RoleId = userRoleSubModuleIVM.RoleID, UserRolewiseSubModules = userRolewiseSubModule });
            }
            ViewBag.Msg = "Update Successfully";

            List<SelectListItemModel> items = await PopulateModule();
            UserRoleSubModuleIVM model = new UserRoleSubModuleIVM();
            model.RoleID = Convert.ToInt32(userRoleSubModuleIVM.RoleID);
            model.selectListItems = items;

            return PartialView("_SubModuleAccess", model);
        }

        [HttpPost]
        public async Task<JsonResult> AddReportAccess(string reportAccess)
        {
            if(reportAccess != "[]")
            {
                List<ReportAccess> ra = JsonConvert.DeserializeObject<List<ReportAccess>>(reportAccess);
                await _mediator.Send(new AddReportAccessCommand { RoleId = ra.FirstOrDefault().RoleId, ReportAccesses = ra });
                return Json(0);
            }
            return Json(0);
        }


        public async Task<IActionResult> UserAccess(int? Id)
        {
            var ComId = _global.GetCompID();
            var UserRole = await _mediator.Send(new GetUserRoleByIdQuery() { RoleID = Convert.ToInt32(Id) });
            ViewBag.RoleName = UserRole?.RoleName.ToString();
            ViewBag.RoleId = UserRole?.RoleID.ToString();
            ViewBag.ModuleId = await _dropdown.ModuleDropdown(ComId);
            ViewBag.SubModuleId = await _dropdown.SubModuleDropdown(ComId);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UserAccess(RoleWiseUserAccessToolsVM roleWiseUserAccessTools, IFormCollection formCollection)
        {
            if(roleWiseUserAccessTools.ModuleID > 0)
            {
                var ModuleId = roleWiseUserAccessTools.ModuleID;
                var RoleID = roleWiseUserAccessTools.RoleID;
                var SubModuleId = roleWiseUserAccessTools.SubModuleID;

                List<UserAccessList> UserAccessList = new List<UserAccessList>();
                string[] ids = formCollection["ID"].ToString().Split(new char[] { ',' });
                foreach (string id in ids)
                {
                    // Get the posted values by using `grid ID` + `column name` + `unique index` syntax
                    string UAToolID = id;
                    string SubMenu = formCollection["SubMenu_" + id].ToString().Split(new char[] { ',' })[0];
                    string PageName = formCollection["PageName_" + id].ToString().Split(new char[] { ',' })[0];
                    string isView = formCollection["View_" + id].ToString().Split(new char[] { ',' })[0];
                    string isEdit = formCollection["Edit_" + id].ToString().Split(new char[] { ',' })[0];
                    string isSave = formCollection["Save_" + id].ToString().Split(new char[] { ',' })[0];
                    string isDelete = formCollection["Delete_" + id].ToString().Split(new char[] { ',' })[0];
                    string isApproved = formCollection["Approved_" + id].ToString().Split(new char[] { ',' })[0];

                    // var submoduleid = await _mediator.Send(new GetSubModuleByUaToolIdQuery() { UAToolid = Convert.ToInt32(UAToolID) });
                    // Do whatever to fit your needs, such as save to database
                    UserAccessList.Add(new UserAccessList
                    {
                        UAtoolID = Convert.ToInt32(UAToolID),
                        SubModuleID = SubModuleId == 0 ? null : SubModuleId,
                        Accesstools = SubMenu,
                        UAVeiw = Convert.ToBoolean(Convert.ToInt32(!string.IsNullOrEmpty(isView) ? isView : 0)),
                        UASave = Convert.ToBoolean(Convert.ToInt32(!string.IsNullOrEmpty(isSave) ? isSave : 0)),
                        UAEdit = Convert.ToBoolean(Convert.ToInt32(!string.IsNullOrEmpty(isEdit) ? isEdit : 0)),
                        UAdelete = Convert.ToBoolean(Convert.ToInt32(!string.IsNullOrEmpty(isDelete) ? isDelete : 0)),
                        UApproved = Convert.ToBoolean(Convert.ToInt32(!string.IsNullOrEmpty(isApproved) ? isApproved : 0)),
                        RoleID = RoleID,
                        AddedBy = roleWiseUserAccessTools.AddedBy,
                        AddedDate = roleWiseUserAccessTools.AddedDate,
                        ModuleID = ModuleId
                    });
                }
                await _mediator.Send(new CreateRoleWiseModuleAccessCommand() { UserAccessList = UserAccessList, ModuleId = ModuleId, RoleId = RoleID, SubModuleId = SubModuleId });

                return RedirectToAction("userAccess", new { id = RoleID });
            }
            else
            {
                var ComId = _global.GetCompID();
                var UserRole = await _mediator.Send(new GetUserRoleByIdQuery() { RoleID = Convert.ToInt32(roleWiseUserAccessTools.RoleID) });
                ViewBag.RoleName = UserRole.RoleName.ToString();
                ViewBag.RoleId = UserRole.RoleID.ToString();
                ViewBag.ModuleId = await _dropdown.ModuleDropdown(ComId);
                ViewBag.SubModuleId = await _dropdown.SubModuleDropdown(ComId);
                ViewBag.ErrorMsg = "Sub Module Not Selected.";
                return RedirectToAction("userAccess", new { id = UserRole.RoleID });
            }
            
        }


        [HttpPost]
        public async Task<JsonResult> MainModuleLoadData(string RoleId)
        {
            List<JsonMainModuleViewModel> JsonMainModuleViewModel = new List<JsonMainModuleViewModel>();
            List<SelectListItemModel> items = await PopulateModule();
            var j = 1;
            for (var i = 0; i < items.Count(); i++)
            {
                JsonMainModuleViewModel.Add(new JsonMainModuleViewModel
                {
                    ModuleID = Convert.ToInt32(items[i].Value),
                    Description = "",
                    ModuleName = items[i].Text,
                    isStatus = await GetSelectModule(Convert.ToInt32(items[i].Value), RoleId)
                });
                j += 1;
            }
            return Json(data: JsonMainModuleViewModel);
        }
        [HttpPost]
        public async Task<JsonResult> SubModuleLoadData(string RoleId)
        {
            List<JsonSubModuleVM> JsonSubModuleVM = new List<JsonSubModuleVM>();
            List<SubModuleVM> items = await PopulateSubModule();
            foreach (var sm in items)
            {
                JsonSubModuleVM.Add(new JsonSubModuleVM
                {
                    SubModuleID = Convert.ToInt32(sm.SubModuleID),
                    ModuleID = Convert.ToInt32(sm.ModuleID),
                    Description = sm.Description,
                    SubModuleName = sm.SubModuleName,
                    ModuleName = sm.ModuleName,
                    isStatus = await GetSelectSubModule(Convert.ToInt32(sm.SubModuleID), RoleId)
                });
            }

            return Json(data: JsonSubModuleVM);
        }

        public async Task<bool> GetSelectModule(int Mid, string RoleID)
        {
            var UserModule = await _mediator.Send(new GetAllUserRoleWiseModuleQuery() { RoleID = Convert.ToInt32(RoleID) });
            var HasValue = UserModule.Where(x => x.ModuleID == Mid).ToList();
            if (HasValue != null)
            {
                if (HasValue.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public async Task<bool> GetSelectSubModule(int SMid, string RoleID)
        {
            var UserModule = await _mediator.Send(new GetAllUserRoleWiseSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            var HasValue = UserModule.Where(x => x.SubModuleID == SMid).ToList();
            if (HasValue != null)
            {
                if (HasValue.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }



        [HttpPost]
        public async Task<JsonResult> ModuleLoadData(string RoleId, string ModuleId, string SubModuleId)
        {
            var tableData = await _mediator.Send(new GetRoleWiseModuleAccessQuery { RoleId = Convert.ToInt32(RoleId), ModuleID = Convert.ToInt32(ModuleId), SubModuleID = Convert.ToInt32(SubModuleId) });
            List<JsonUserAccessViewModel> JsonUserAccessViewModel = new List<JsonUserAccessViewModel>();
            if (tableData != null)
            {
                if (tableData.Count > 0)
                {
                    foreach (var item in tableData)
                    {
                        JsonUserAccessViewModel.Add(new JsonUserAccessViewModel
                        {
                            UAToolID = item.uatoolid,
                            SubMenu = item.UseraccesstoolNM,
                            PageName = item.UAPage,
                            isEdit = item.UAEdit,
                            isView = item.UAVeiw,
                            isSave = item.UASave,
                            isDelete = item.UAdelete,
                            isApproved = item.UApproved
                        });
                    }
                }
                else
                {
                    JsonUserAccessViewModel.Add(new JsonUserAccessViewModel
                    {
                        UAToolID = 0,
                        SubMenu = "",
                        PageName = "",
                        isEdit = false,
                        isView = false,
                        isSave = false,
                        isDelete = false,
                        isApproved = false
                    });
                }
            }
            return Json(data: JsonUserAccessViewModel);
        }


        public async Task<List<SelectListItemModel>> PopulateModule()
        {
            var GetSource = await _mediator.Send(new GetAllMainModuleQuery());
            List<SelectListItemModel> MainModule = new List<SelectListItemModel>();
            foreach (var item in GetSource)
            {
                MainModule.Add(new SelectListItemModel
                {
                    Text = item.ModuleName,
                    Value = item.ModuleID.ToString(),
                });
            }
            return MainModule;
        }

        public async Task<List<SubModuleVM>> PopulateSubModule()
        {
            var GetSource = await _mediator.Send(new GetAllSubModuleQuery());
            List<SubModuleVM> SubModule = new List<SubModuleVM>();
            foreach (var item in GetSource)
            {
                SubModule.Add(new SubModuleVM
                {
                    SubModuleName = item.SubModuleName,
                    SubModuleID = item.SubModuleID,
                    ModuleID = item.ModuleID,
                    ModuleName = item.MainModule.ModuleName,
                });
            }
            return SubModule;
        }


        public async Task PopulateModules()
        {
            var GetSource = await _mediator.Send(new GetAllMainModuleQuery());
            List<MainModule> cl = new List<MainModule>();
            cl = GetSource;
            ViewBag.SelectListItem = cl;
        }
    }
}
