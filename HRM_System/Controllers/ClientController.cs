using Application.SupportiveBL.Context;
using Application.Tasks.Commands.CClient;
using Application.Tasks.Commands.CCompany;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Handlers.HClient;
using Application.Tasks.Handlers.HCompany;
using Application.Tasks.Queries.QClient;
using Application.Tasks.Queries.QCompany;
using UKHRM.Helpers;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;
using DataEncryption = UKHRM.Helper.DataEncryption;
using Microsoft.AspNetCore.Authorization;


namespace UKHRM.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly IAuthorizationContext authContext;

        public ClientController(IMediator mediator, IGlobalHelper global, IAuthorizationContext authContext)
        {
            _mediator = mediator;
            _global = global;
            this.authContext = authContext;
            this.authContext.RoleId = _global.GetRoleID();
            this.authContext.UserId = _global.GetUserID();
            this.authContext.UserName = _global.GetUserName();
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
                var UserId = DataEncryption.DecryptString(Request.Cookies["UserID"]);
                // Object Diclaration
                var companies = new List<ClientViewModel>();
                var OrgId = _global.GetOrgId();
                var ClientId = _global.GetClientId();
                var RoleType = _global.GetRoleType();
                companies = await _mediator.Send(new SP_Dt_ClientListQuery() { DisplayLength = Convert.ToInt32(length), Start = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ClientId = ClientId, RoleType = RoleType });

                if (companies.Count != 0)
                {
                    if (companies.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = companies.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = companies });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
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
            ClientViewModel clientViewModel = new ClientViewModel();
            clientViewModel.ClientID = 0;
            return View(clientViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientViewModel module)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (module.ClientID > 0)
                    {
                        if (module.Logofile != null)
                        {
                            //Getting FileName
                            var fileName = Path.GetFileName(module.Logofile.FileName);
                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);
                            // concatenating  FileName + FileExtension
                            var newFileName = string.Concat(myUniqueFileName, fileExtension);
                            // Combines two strings into a path.
                            var filepath =
                            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UpImages")).Root;
                            var saveMain = "/UpImages/" + "clt_" + newFileName;
                            // Main Image
                            using (FileStream fs = System.IO.File.Create(filepath + $@"\{"clt_" + newFileName}"))
                            {
                                await module.Logofile.CopyToAsync(fs);
                                fs.Flush();
                            }
                            module.Logo = saveMain;
                        }
                        else
                        {
                            var oldData = await _mediator.Send(new GetClientByIdQuery { ClientID = module.ClientID });
                            module.Logo = oldData.Logo;
                        }
                        //company.CompID = Convert.ToInt32(company.CompID);
                        await _mediator.Send(new UpdateClientCommand { ClientViewModel = module });

                        #region Edit Delete Info
                        var json = JsonConvert.SerializeObject(module);
                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = module.ClientID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Upsert} Client", DocumentReferance = json });
                        #endregion
                    }
                    else
                    {
                        // whole image upload function transferred to the handler

                        await _mediator.Send(new CreateClientCommand() { ClientViewModel = module });

                        #region Edit Delete Info
                        var sjson = JsonConvert.SerializeObject(module);
                        await _mediator.Send(new CreateTransactionLogCommand { TransectionID = module.ClientID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Client", DocumentReferance = sjson });
                        #endregion
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
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InviteClient(InviteClient invite)
        {
            invite.AddedById = Convert.ToInt32(_global.GetUserID());
           var message = await _mediator.Send(new InviteClientCommand { InviteClient = invite });
            return Json(message);
        }

        [HttpGet]
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
            var OrgId = _global.GetOrgId();
            ViewBag.Action = "Edit";
            if (id == null)
            {
                return Redirect("/error/404");
            }
            var clients = await _mediator.Send(new GetClientByIdQuery { ClientID = Convert.ToInt32(id), OrgId = OrgId });

            if (clients == null)
            {
                return Redirect("/error/404");
            }
            return View("Create", clients);
        }

        //[HttpPost]
        public async Task<IActionResult> GetClientById(int ClientId)
        {
            var client = await _mediator.Send(new GetClientByIdQuery() { ClientID = ClientId });
            return Json(client);
        }

        [HttpPost]
        public async Task<IActionResult> _ClientDetails(ClientViewModel data)
        {
            return PartialView("_ClientDetails", data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail(string Email)
        {
            var message = await _global.CheckEmail(Email);
            return Json(message);
        }
    }
}
