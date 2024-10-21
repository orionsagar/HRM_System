using Application.Tasks.Commands.CMainModule;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QMainModule;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UKHRM.Controllers
{
    [Authorize]
    public class ModuleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(IMediator mediator, ILogger<ModuleController> logger, IGlobalHelper global)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
            _logger = logger;
            _global = global;
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                // getting all Customer data  
                //var customerData = (from tempcustomer in _context.CustomerTB
                //                    select tempcustomer);


                //List<Company> customerData = await _mediator.Send(new CList.Query());
                var customerData = await _mediator.Send(new GetAllMainModuleQuery());

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(x => x.ModuleName == sortColumn + " " + sortColumnDirection).ToList();
                    customerData = customerData.OrderBy(x => x.ModuleName == sortColumn).ToList();
                }

                //Sorting  
                if (!string.IsNullOrEmpty(sortColumnDirection))
                {
                    customerData = customerData.OrderBy(x => x.ModuleName == sortColumnDirection).ToList();
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    //customerData = (List<MainModule>)customerData.Where(m => m.ModuleName != null && m.ModuleName.ToUpper().Contains(searchValue.ToUpper()));
                    customerData = customerData.Where(m => m.ModuleName.ToUpper().Contains(searchValue.ToUpper())).ToList();
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MainModule module)
        {
            try
            {
                if (module.ModuleID != 0)
                {
                    await _mediator.Send(new UpdateMainModuleCommand { MainModule = module });

                    var json = JsonConvert.SerializeObject(module);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = module.ModuleID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} Module", DocumentReferance = json });
                }
                else
                {
                    await _mediator.Send(new CreateMainModuleCommand() { MainModule = module });

                    var json = JsonConvert.SerializeObject(module);

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = module.ModuleID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Module", DocumentReferance = json });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
                return View(module);
            }
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
            var module = await _mediator.Send(new GetMainModuleByIdQuery { ModuleID = Convert.ToInt32(id) });
            if (module == null)
            {
                return Redirect("/error/404");
            }
            return View("Create", module);
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
            var tableData = await _mediator.Send(new GetMainModuleByIdQuery { ModuleID = Convert.ToInt32(Id) });

            if (tableData != null)
            {
                await _mediator.Send(new DeleteMainModuleCommand() { Id = Convert.ToInt32(Id) });

                var json = JsonConvert.SerializeObject(Id);

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} Module", DocumentReferance = json });
            }
            return Json(data: tableData);
        }
    }
}
