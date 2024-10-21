using Application.Tasks.Queries.QEmployee;
using UKHRM.Helpers;
using Business.HR;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UKHRM.Controllers.HR
{
    [Authorize]
    public class EditDeleteInfoController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IEmployeeBL _employeeBL;
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditDeleteInfoController(CommonDropdown dropdown, IEmployeeBL employeeBL, IMediator mediator, IGlobalHelper helper, IWebHostEnvironment webHostEnvironment = null)
        {
            _dropdown = dropdown;
            _employeeBL = employeeBL;
            _mediator = mediator;
            _global = helper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> IndexAsync()
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
            //ViewBag.Employees = await _mediator.Send(new GetAllEmployeeQuery() { CompId = compId });
            ViewBag.User = await _dropdown.UserDropdown(compId,OrgId);
            ViewBag.CommandType = await _dropdown.CommandTypeDropdown(compId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var search = Request.Form["search[value]"].FirstOrDefault();
                var LogUserId = Convert.ToInt32(Request.Form["UserId"].FirstOrDefault() ?? "0");
                var FromDate = Convert.ToDateTime(Request.Form["FromDate"].FirstOrDefault() ?? DateTime.Now.ToString("dd-MM-yyyy"));
                var ToDate = Convert.ToDateTime(Request.Form["ToDate"].FirstOrDefault() ?? DateTime.Now.ToString("dd-MM-yyyy"));
                var Controller = Convert.ToString(Request.Form["Controller"].FirstOrDefault() ?? "");
                var Action = Convert.ToString(Request.Form["Action"].FirstOrDefault() ?? "");
                var CommandType = Convert.ToString(Request.Form["CommandType"].FirstOrDefault() ?? "");

                var totalrecord = 0;
                //var UserId = _global.GetUserID(); ;
                //var Role = DataEncryption.DecryptString(Request.Cookies["Role"]);
                var ComId = _global.GetCompID();

                var data = Enumerable.Empty<EditDeleteInfoVM>();
                EditDeleteInfoFilterVM filterVM = new EditDeleteInfoFilterVM()
                {
                    UserId = LogUserId,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    Controller = Controller,
                    Action = Action,
                    CommandType = CommandType,
                };

                data = await _mediator.Send(new GetEditDeleteInfoListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ComId = ComId, EditDeleteInfoFilterVM = filterVM });

                if (data.Any())
                {
                    totalrecord = data.FirstOrDefault()?.TOTALCOUNT ?? 0;
                    //if (data.FirstOrDefault().TotalCount != 0)
                    //{
                    //    totalrecord = data.FirstOrDefault().TotalCount;
                    //}
                    //else
                    //{
                    //    totalrecord = 0;
                    //}
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
