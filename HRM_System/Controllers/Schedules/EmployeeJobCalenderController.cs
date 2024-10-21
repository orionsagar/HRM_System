using Application.Tasks.Queries.QEmployeeJobCalender;
using AutoMapper;
using UKHRM.Helpers;
using Business.Schedule;
using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UKHRM.Controllers.Schedules
{
    [Authorize]
    public class EmployeeJobCalendarController : Controller
    {
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployeeJobCalendarBL _employeeJobCalenderBL;
        //private readonly ILogger _logger;

        public EmployeeJobCalendarController(CommonDropdown dropdown, IGlobalHelper global, IMediator mediator, IEmployeeJobCalendarBL EmployeeJobCalenderBL, IMapper mapper)
        {
            _dropdown = dropdown;
            _global = global;
            _mediator = mediator;
            _employeeJobCalenderBL = EmployeeJobCalenderBL;
            _mapper = mapper;
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
            var compId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.JobCalender =await _dropdown.JobCalendarDropdown(compId, OrgId);
            return View();
        }

        //public async Task<IActionResult> LoadData()
        //{
        //    try
        //    {
        //        //var moduleid = Convert.ToInt32(Request.Form["ModuleId"].FirstOrDefault());
        //        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        //        // Skip number of Rows count  
        //        var start = Request.Form["start"].FirstOrDefault();
        //        // Paging Length 10,20  
        //        var length = Request.Form["length"].FirstOrDefault();
        //        // Sort Column Name  
        //        var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
        //        // Sort Column Direction (asc, desc)  
        //        var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        // Search Value from (Search box)  
        //        var search = Request.Form["search[value]"].FirstOrDefault();
        //        int recordsTotal = 0;
        //        var comid = _global.GetCompID();
        //        var EmployeeJobCalender = await _mediator.Send(new SP_Dt_EmployeeJobCalenderListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, ComId = comid });

        //        //total number of rows counts   
        //        if (EmployeeJobCalender.Count > 0)
        //        {
        //            recordsTotal = EmployeeJobCalender.FirstOrDefault().TOTALCOUNT;
        //        }
        //        //Returning Json Data  
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = EmployeeJobCalender });
        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine(ex.Message.ToString());
        //        //_logger.LogInformation($"Task was cancelled" + ex.Message.ToString());
        //        throw;
        //    }
        //}

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadEmployee(EmpJobCalendarFilter filter)
        {
            try
            {
                var OrgId = _global.GetOrgId();
                var ClientId = _global.GetClientId();
                filter.OrgId = OrgId;
                filter.ClientId = ClientId;
                var data = await _mediator.Send(new GetEmpByJobCalenderQuery { EmpJobCalenderFilter = filter });
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<EmployeeJobCalendar> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var OrgId = _global.GetOrgId();
                    var ClientId = _global.GetClientId();

                    foreach (var item in model)
                    {
                        if (item.EmpJobCalenderId > 0)
                        {
                            item.UpdatedBy = _global.GetUserID();
                            item.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            item.AddedBy = _global.GetUserID();
                            item.AddedDate = DateTime.Now;
                        }
                        item.OrgId = OrgId;
                        //item.ClientId = ClientId;
                        item.CompId = _global.GetCompID();
                    }

                    

                    var status = await _employeeJobCalenderBL.Upsert(model);
                    return Json(status);
                }

                return Json(new BLStatus { IsError = true, Message = "Submited data not valid", StatusCode = "400" });
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message=ex.InnerException.Message, StatusCode="500" }) ;
            }           
            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteByEmpId(List<int> empIds)
        {
            try
            {
                var status = await _employeeJobCalenderBL.DeleteByEmpId(empIds);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int empJobCalenderId)
        {
            try
            {
                var status = await _employeeJobCalenderBL.Delete(empJobCalenderId);
                return Json(status);
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to delete." });
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetByEmpId(int empId)
        {
            try
            {
                if (empId < 1) return BadRequest();

                var data = await _mediator.Send(new GetEmpByJobCalenderByEmpIdQuery { EmpId = empId });
                return Json(new BLStatus { Data=data});
            }
            catch (Exception ex)
            {
                return Json(new BLStatus { IsError = true, Message = "Unable to get data." });
            }
        }


    }
}
