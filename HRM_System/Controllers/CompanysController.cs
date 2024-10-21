using Application.Tasks.Commands.CCompany;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Handlers.HCompany;
using Application.Tasks.Queries.QCompany;
using UKHRM.Helpers;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using DataEncryption = Utility.DataEncryption;

namespace UKHRM.Controllers
{
    //[Authorize]
    public class CompanysController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        public CompanysController(IMediator mediator, IGlobalHelper global)
        {
            _mediator = mediator;
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
                var companies = new List<CompanyViewModel>();

                companies = await _mediator.Send(new SP_Dt_CompanyListQuery() { DisplayLength = Convert.ToInt32(length), Start = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search });

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
            return View();
        }
        public async Task<IActionResult> Create_Org()
        {
            //#region Access
            //var roleid = _global.GetRoleID();
            //var controller = RouteData.Values["controller"];
            //var action = RouteData.Values["action"];
            //var url = $"{controller}/{action}";
            //ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
            //ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
            //ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
            //ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
            //#endregion
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                if (company.CompID > 0)
                {
                    if (company.Logofile != null)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(company.Logofile.FileName);
                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);
                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);
                        // Combines two strings into a path.
                        var filepath =
                        new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UpImages")).Root;
                        var saveMain = "/UpImages/" + "cmp_" + newFileName;
                        // Main Image
                        using (FileStream fs = System.IO.File.Create(filepath + $@"\{"cmp_" + newFileName}"))
                        {
                            await company.Logofile.CopyToAsync(fs);
                            fs.Flush();
                        }
                        company.Logo = saveMain;
                    }
                    else
                    {
                        var oldData = await _mediator.Send(new GetCompanyByIdQuery { CompID = company.CompID });
                        company.Logo = oldData.Logo;
                    }
                    //company.CompID = Convert.ToInt32(company.CompID);
                    await _mediator.Send(new UpdateCompanyCommand { CompanyViewModel = company });

                    #region Edit Delete Info
                    var json = JsonConvert.SerializeObject(company);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = company.CompID.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Upsert} Company", DocumentReferance = json });
                    #endregion
                }
                else
                {
                    if (company.Logofile != null)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(company.Logofile.FileName);

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        // Combines two strings into a path.
                        var filepath =
                        new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UpImages")).Root;

                        var saveMain = "/UpImages/" + "cmp_" + newFileName;
                        // Main Image
                        using (FileStream fs = System.IO.File.Create(filepath + $@"\{"cmp_" + newFileName}"))
                        {
                            await company.Logofile.CopyToAsync(fs);
                            fs.Flush();
                        }
                        company.Logo = saveMain;
                    }
                    await _mediator.Send(new CreateCompanyCommand() { CompanyViewModel = company });
                }

                #region Edit Delete Info
                var sjson = JsonConvert.SerializeObject(company);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = company.CompID.ToString(), CommandType = Enums.commandtype.Create.ToString(), TransStatement = $"{Enums.commandtype.Create} Company", DocumentReferance = sjson });
                #endregion
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(company);
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
            var companys = await _mediator.Send(new GetCompanyByIdQuery { CompID = Convert.ToInt32(id) });
            //CompanyViewModel companyViewModel = new CompanyViewModel()
            //{
            //    CompID = companys.CompID,
            //    Name = companys.Name,
            //    Address = companys.Address,
            //    CompType = companys.CompType,
            //    BusinessType = companys.BusinessType,
            //    Attention = companys.Attention,
            //    City = companys.City,
            //    State = companys.State,
            //    HotNumber = companys.HotNumber,
            //    BKashNumber = companys.BKashNumber,
            //    Phone1 = companys.Phone1,
            //    Phone2 = companys.Phone2,
            //    Email1 = companys.Email1,
            //    Email2 = companys.Email2,
            //    BusinessHour = companys.BusinessHour,
            //    Website = companys.Website,
            //    Country = companys.Country,
            //    Facebook = companys.Facebook,
            //    Instagram = companys.Instagram,
            //    Linkedin = companys.Linkedin,
            //    twitter = companys.twitter,
            //    GooglePlus = companys.GooglePlus,
            //    Logo = companys.Logo,
            //    FBPixelScriptHeader = companys.FBPixelScriptHeader,
            //    GAScriptHeader = companys.GAScriptHeader
            //};          

            if (companys == null)
            {
                return Redirect("/error/404");
            }
            return View("Create", companys);
        }
    }
}
