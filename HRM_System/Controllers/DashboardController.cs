using Business;
using Business.HR;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using UKHRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Tasks.Queries.QMainModule;
using MediatR;
using Application.Tasks.Queries.QUserInfo;
using Application.Tasks.Queries.QOrganisation;
using Application.Tasks.Queries.QClient;
using Application.Tasks.Commands.CClient;
using Application.Tasks.Commands.CTransactionLog;
using Newtonsoft.Json;
using Utility;
using Application.Tasks.Commands.CRTWC;

namespace UKHRM.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardBL _dashboard;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IAuthBL _auth;
        private readonly CommonDropdown _common;


        public DashboardController(ILogger<HomeController> logger, IDashboardBL dashboard, IGlobalHelper global, IMediator mediator, IAuthBL auth, CommonDropdown common)
        {
            _logger = logger;
            _dashboard = dashboard;
            _global = global;
            _mediator = mediator;
            _auth = auth;
            _common = common;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[Route("Folder/ControllerName/ActionName")]
        public async Task<IActionResult> ModuleDashboard()
        {
            try
            {

                var RoleID = _global.GetRoleID();
                ViewBag.Module = await _mediator.Send(new GetMenuModuleQuery() { RoleID = Convert.ToInt32(RoleID) });
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //NEW UPDATION FOR DASHBOARDS
        public async Task<IActionResult> SubscriptionDashboard(int ModuleId)
        {
            try
            {
                //var RoleID = _global.GetRoleID();
                //ViewBag.SubModule_Menu = await _auth.Get_SubModule_Menu_By_ModuleId(ModuleId,Convert.ToInt32(RoleID));
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> OrganizationDashboard(int ModuleId)
        {
            try
            {
                var RoleID = _global.GetRoleID();
                ViewBag.SubModule_Menu = await _auth.Get_SubModule_Menu_By_ModuleId(ModuleId, Convert.ToInt32(RoleID));
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ActionResult> ActiveEmployee(string effectdate)
        {
            var effect = _global.DateConvertion(effectdate);
            var activeemp = await _dashboard.GetTotalActiveEmployee(effectdate);

            return Json(data: activeemp);
        }
        public async Task<ActionResult> PresentEmployee(string effectdate)
        {
            var effect = _global.DateConvertion(effectdate);
            var presentemp = await _dashboard.GetTotalPresentEmployee(effect);
            return Json(data: presentemp);
        }
        public async Task<ActionResult> AbsentEmployee(string effectdate)
        {
            var effect = _global.DateConvertion(effectdate);
            var absentemp = await _dashboard.GetTotalAbsentEmployee(effect);
            return Json(data: absentemp);
        }
        public async Task<ActionResult> LeaveEmployee(string effectdate)
        {
            var effect = _global.DateConvertion(effectdate);
            var leaveemp = await _dashboard.GetTotalLeaveEmployee(effect);
            return Json(data: leaveemp);
        }
        public async Task<ActionResult> EmployeeStatusChart()
        {
            var statusdata = await _dashboard.EmployeeStatusChart();
            return Json(data: statusdata);
        }
        public async Task<ActionResult> MonthlyAttendanceForChart(string effectdate)
        {
            List<ChartVM> charts = new List<ChartVM>();

            var effect = _global.DateConvertionCSharp(effectdate);
            DateTime StartDate = new DateTime(effect.Year, effect.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(effect.Year, effect.Month);
            DateTime EndDate = new DateTime(effect.Year, effect.Month, daysInMonth);

            var data = await _dashboard.MonthlyAttendanceForChart(StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
            var types = data.Select(d => d.Type).Distinct().OrderBy(a => a).ToList();
            var datelist = data.Select(d => d.AttDate).Distinct().ToList();
            foreach (var type in types)
            {
                ChartVM chart = new ChartVM();
                //Chart1VM chart1 = new Chart1VM();
                chart.Name = type;
                var res = data.Where(r => r.Type == type).ToList();
                foreach (var a in res)
                {
                    //chart1.data.Add(a.AttDate, a.Total);                    
                    chart.data.Add(a.Total);
                }
                charts.Add(chart);
            }
            return Json(new { datelist, charts });
        }

        public IActionResult Create()
        {
            return View();
        }


        public IActionResult List()
        {
            return View();
        }

        // ===========================================================================================================================================
        // ==================== Below are the codes that require changes when implementing each page currently its dummy =============================
        // ===========================================================================================================================================

        //Please Find the index controller and do the required changes above below newly index is NOT been created


        public IActionResult SingleForm()
        {
            return View();
        }
        public IActionResult TablePage()
        {
            return View();
        }

        public IActionResult TwoForms()
        {
            return View();
        }

        public IActionResult VerticleForms()
        {
            return View();
        }

        public IActionResult EmailVerify()
        {
            return View();
        }


        public async Task<IActionResult> Test()
        {
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            var ClientID = _global.GetClientId();
            ViewBag.EmpId = await _common.EmployeeDropdown(ComId, OrgId);
            var rtw = new RightToWorkCheck();
            //var rtwstep = new List<RTWStap1>();
            //rtwstep = await _global.RTWStap1();
            //rtw.RTWStap1 = rtwstep;
            return View(rtw);
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
                // Object Diclaration
                var organisations = new List<OrganisationVM>();

                organisations = await _mediator.Send(new SP_Dt_OrganisationListQuery() { DisplayLength = Convert.ToInt32(length), Start = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, ClientId = ClientId, OrgId = orgid });

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


        public async Task<IActionResult> ActiveClient()
        {
            var clients = await _mediator.Send(new GetClientByStatusQuery { Status = "Active" });
            return View(clients);
        }
        public async Task<IActionResult> InactiveClient()
        {
            var clients = await _mediator.Send(new GetClientByStatusQuery { Status = "Inactive" });
            return View(clients);
        }
        public async Task<IActionResult> PendingClient()
        {
            var clients = await _mediator.Send(new GetClientByStatusQuery { Status = "Pending" });
            return View(clients);
        }


       
    }
}

