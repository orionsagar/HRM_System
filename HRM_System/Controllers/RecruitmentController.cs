using Application.Tasks.Commands.CRecruitment;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QRecruitment;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;
using static Domains.ViewModels.RecruitmentVM;

namespace UKHRM.Controllers
{
    public class RecruitmentController : Controller
    {
        public readonly CommonDropdown _commn;
        public readonly IGlobalHelper _global;
        public readonly IMediator _mediator;

        public RecruitmentController(CommonDropdown commn, IGlobalHelper global, IMediator mediator)
        {
            _commn = commn;
            _global = global;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> JobList()
        {
            var OrgId = _global.GetOrgId();
            var ClientId = _global.GetClientId();
            ViewBag.OrgId = await _commn.OrganisationDropdown(OrgId, ClientId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JobListLoad()
        {
            try
            {
                var OrgId = Request.Form["OrgId"].FirstOrDefault();
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
                var clientid = _global.GetClientId();
                var roletype = _global.GetRoleType();
                OrgId = (OrgId == "" || OrgId == null) ? "0" : OrgId;
                var param = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    CompId = Convert.ToInt32(comid),
                    OrgId = OrgId == "" ? 0 : Convert.ToInt32(OrgId),
                    ClientId = clientid,
                    RoleType = roletype
                };
                var payScaleTypes = await _mediator.Send(new SP_Dt_JobListQuery() { Param = param });

                //total number of rows counts   
                if (payScaleTypes.Count > 0)
                {
                    recordsTotal = payScaleTypes.FirstOrDefault().TOTALCOUNT;
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = payScaleTypes });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> JobAdd()
        {
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            var ClientId = _global.GetClientId();
            ViewBag.DeptId = await _commn.DepartmentDropdown(ComId, OrgId);
            ViewBag.OrgId = await _commn.OrganisationDropdown(OrgId, ClientId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JobAdd(JobVM job)
        {
            var OrgId = _global.GetOrgId();
            try
            {
                var jobid = await _mediator.Send(new JobAddCommand { JobVM = job });
                var json = JsonConvert.SerializeObject(job);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = job.JobId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = $"{Enums.commandtype.Create} Job", DocumentReferance = json });
                return Json(jobid);
            }
            catch (Exception ex)
            {
                ViewBag.DeptId = await _commn.DepartmentDropdown(job.CompId,OrgId);
                return Json(ex.InnerException.Message);
            }
        }

        public async Task<JsonResult> GetSOCCode()
        {
            var joblist = await _commn.SOCCodeDropdown();
            return Json(joblist);
        }

    }
}
