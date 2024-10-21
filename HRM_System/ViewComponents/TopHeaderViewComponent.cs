using Application.Tasks.Queries.QCompany;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using static System.Net.WebRequestMethods;
using System;

namespace UKHRM.ViewComponents
{
    public class TopHeaderViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _common;
        private readonly ImageChecker _imgchecker;
        private readonly IWebHostEnvironment _env;

        public TopHeaderViewComponent(IMediator mediator, CommonDropdown common, ImageChecker imgchecker, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _common = common;
            _imgchecker = imgchecker;
            _env = env;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {            
            var companyQ = await _mediator.Send(new GetAllCompanyQuery());
            if (companyQ != null)
            {
                foreach (var item in companyQ)
                {
                    var logo = !string.IsNullOrEmpty(item.Logo) ? item.Logo : "";
                    
                    ViewBag.CompanyName = item.Name;
                    ViewBag.Address = item.Address;

                    bool imageExists = _imgchecker.ImageExists(logo);
                    if (imageExists)
                    {
                        ViewBag.CompanyLogo = logo;
                    }
                    else
                    {
                        string imagePath = Path.Combine(_env.WebRootPath, "/UpImages/UltLogo.png");
                        ViewBag.CompanyLogo = imagePath;
                    }
                }
            }
            bool check = false;
            var data = HttpContext.Request.Cookies["UserID"];
            check = data != "" ? DataEncryption.IsEncrypted(data) : false;
            var userid = check ? DataEncryption.DecryptString(data) : data;
            //var userid = !string.IsNullOrEmpty(HttpContext.Request.Cookies["UserID"]) ? DataEncryption.DecryptString(HttpContext.Request.Cookies["UserID"]) : "";
            
            ViewBag.UserId = userid;

            check = false;
            var data1 = HttpContext.Request.Cookies["Username"];
            check = data != "" ? DataEncryption.IsEncrypted(data) : false;
            var username = check ? DataEncryption.DecryptString(data1) : data;

            ViewBag.UserName = username;

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

            return View("TopHeader");
        }
    }
}
