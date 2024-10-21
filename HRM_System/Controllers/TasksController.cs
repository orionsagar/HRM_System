using Application.Tasks.Commands.CTasks;
using Application.Tasks.Handlers.HCompany;
using Application.Tasks.Queries.QTasks;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UKHRM.Helper;

namespace UKHRM.Controllers
{
    public class TasksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;

        public TasksController(IMediator mediator, IGlobalHelper global)
        {
            _mediator = mediator;
            _global = global;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LoadData()
        {
            try
            {
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
                var OrgId = _global.GetOrgId();
                // Object Diclaration
                var Task = new List<TasksVM>();
                var param = new DataTableParamVM
                {
                    DisplayLength = Convert.ToInt32(length),
                    DisplayStart = Convert.ToInt32(start),
                    SortCol = Convert.ToInt32(ordercolumn),
                    SortDir = orderdirection,
                    Search = search,
                    ClientId = ClientId,
                    OrgId = OrgId
                };
                Task = await _mediator.Send(new SP_Dt_TaskListQury() { Param = param });

                if (Task.Count != 0)
                {
                    if (Task.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = Task.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = Task });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tasks tasks)
        {
            try
            {
                var UserId = _global.GetUserID();
                var ClientId = _global.GetClientId();
                var OrgId = _global.GetOrgId();
                if(tasks.TaskId > 0)
                {
                    tasks.UpdatedBy = UserId;
                    tasks.UpdatedDate = DateTime.Now;
                }else
                {
                    tasks.ClientId = ClientId;
                    tasks.OrgId = OrgId;
                    tasks.AddedBy = UserId;
                    tasks.AddedDate = DateTime.Now;
                }
                var task = await _mediator.Send(new AddTaskCommand { Tasks = tasks });
                return Json(tasks);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
                throw;
            }
            
        }

    }
}
