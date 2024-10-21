using Application.Tasks.Queries.QCompany;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Application.Tasks.Queries.QUserInfo;
using System;
using Application.Tasks.Queries.QOrganisation;

namespace UKHRM.ViewComponents
{
    public class TopHeader1ViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _common;
        private readonly ImageChecker _imgchecker;
        private readonly IWebHostEnvironment _env;
        private readonly IGlobalHelper _global;

        public TopHeader1ViewComponent(IMediator mediator, CommonDropdown common, ImageChecker imgchecker, IWebHostEnvironment env, IGlobalHelper global)
        {
            _mediator = mediator;
            _common = common;
            _imgchecker = imgchecker;
            _env = env;
            _global = global;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var companyQ = await _mediator.Send(new GetAllCompanyQuery());
            //if (companyQ != null)
            //{
            //    foreach (var item in companyQ)
            //    {
            //        var logo = !string.IsNullOrEmpty(item.Logo) ? item.Logo : "";

            //        ViewBag.CompanyName = item.Name;
            //        ViewBag.Address = item.Address;

            //        bool imageExists = _imgchecker.ImageExists(logo);
            //        if (imageExists)
            //        {
            //            ViewBag.CompanyLogo = logo;
            //        }
            //        else
            //        {
            //            string imagePath = Path.Combine(_env.WebRootPath, "/UpImages/UltLogo.png");
            //            ViewBag.CompanyLogo = imagePath;
            //        }
            //    }
            //}

            //var userid = !string.IsNullOrEmpty(HttpContext.Request.Cookies["UserID"]) ? DataEncryption.DecryptString(HttpContext.Request.Cookies["UserID"]) : "";

            //ViewBag.UserId = userid;
            //var username = HttpContext.Request.Cookies["Username"];

            ViewBag.UserName = _global.GetUserName();

            //ViewBag.UserId = !string.IsNullOrEmpty(HttpContext.Request.Cookies["UserID"]) ? DataEncryption.DecryptString(HttpContext.Request.Cookies["UserID"]) : "";
            //ViewBag.UserRole = !string.IsNullOrEmpty(HttpContext.Request.Cookies["Role"]) ? DataEncryption.DecryptString(HttpContext.Request.Cookies["Role"]) : "";

            //var userinfo = await _mediator.Send(new GetUserInfoByIdQuery() { UserID = Convert.ToInt32(userid) });
            //var Branch = await _mediator.Send(new GetAllBranchQuery());
            //if (userinfo.BranchID != 0)
            //{
            //    ViewBag.BranchID = userinfo.BranchID;
            //    SelectList lst = new SelectList(Branch, "BranchID", "BranchName", userinfo.BranchID);
            //    ViewBag.Branch = lst;
            //}
            //else
            //{
            //    ViewBag.BranchID = null;
            //    SelectList lst = new SelectList(Branch, "BranchID", "BranchName");
            //    ViewBag.Branch = lst;
            //}


            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["RoleType"]))
            {
                var RoleType = DataEncryption.DecryptString(HttpContext.Request.Cookies["RoleType"]);
                var userid = DataEncryption.DecryptString(HttpContext.Request.Cookies["UserID"]);
                var userinfo = await _mediator.Send(new GetUserInfoByIdQuery() { UserID = Convert.ToInt32(userid) });

                if (RoleType == "Org_Role")
                {
                    var orgid = _global.GetOrgId();
                    var orginfo = await _mediator.Send(new GetOrganisationDataByIdQuery() { OrgId = Convert.ToInt32(orgid) });
                    
                    ViewBag.FullName = userinfo.FirstName + ' ' + userinfo.LastName;
                    ViewBag.Client = orginfo.OrgName;
                    ViewBag.Email = userinfo.Email;
                }
                else if (RoleType == "Client_Role")
                {
                    ViewBag.FullName = userinfo.FirstName + ' ' + userinfo.LastName;
                    ViewBag.Client = "Client/Solicitor";
                    ViewBag.Email = userinfo.Email;
                }
                else if (RoleType == "Platform_Role")
                {
                    ViewBag.FullName = userinfo.FirstName + ' ' + userinfo.LastName;
                    ViewBag.Client = "Platform Admin";
                    ViewBag.Email = userinfo.Email;
                }

            }





            return View("TopHeader1");
        }
    }
}
