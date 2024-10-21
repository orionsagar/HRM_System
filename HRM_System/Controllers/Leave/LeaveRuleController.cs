using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Handlers.HReaveRule;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;

namespace UKHRM.Controllers.Leave
{
    public class LeaveRuleController : Controller
    {
        private readonly IMediator _mediator;
        private readonly CommonDropdown _dropdown;
        private readonly IGlobalHelper _global;
        private readonly IMapper _mapper;

        public LeaveRuleController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global, IMapper mapper)
        {
            _mediator = mediator;
            _dropdown = dropdown;
            _global = global;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var param = new DataTableParamVM
            {
                OrgId = _global.GetOrgId()
            };
            var orgid = _global.GetOrgId();  // Get the OrgId
            var data = await _mediator.Send(new GetAllLeaveRuleQuery { OrgId = orgid });
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.EmpTypeId = await _dropdown.EmployeeTypeDropdown(ComId, OrgId, 0);
            ViewBag.LeaveTypeId = await _dropdown.LeaveTypeDropdown(ComId, OrgId, 0);
            ViewBag.Action = "Add";
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Create(LeaveRuleVM leaveRuleVM)
        //{
        //    leaveRuleVM.EffectiveFrom = _global.DateConvertion(leaveRuleVM.EffectiveFrom);
        //    leaveRuleVM.EffectiveTo = _global.DateConvertion(leaveRuleVM.EffectiveTo);
        //    await _mediator.Send(new UpsertLeaveRuleCommand { LeaveRuleVM = leaveRuleVM });
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> Create(LeaveRuleVM leaveRuleVM)
        {
            // Convert dates using the global helper
            //leaveRuleVM.EffectiveFrom = _global.DateConvertion(leaveRuleVM.EffectiveFrom);
            //leaveRuleVM.EffectiveTo = _global.DateConvertion(leaveRuleVM.EffectiveTo);

            // Send the upsert command and get the affected ID
            var affectedId = await _mediator.Send(new UpsertLeaveRuleCommand { LeaveRuleVM = leaveRuleVM });
            var id = 0;
            var status = "";

            // Determine if the action is an update or create based on the ID
            if (leaveRuleVM.LeaveRuleId > 0)
            {
                id = leaveRuleVM.LeaveRuleId;
                status = "Update";
            }
            else
            {
                id = affectedId;
                status = "Create";
            }

            // Log the transaction
            await _mediator.Send(new CreateTransactionLogCommand
            {
                TransectionID = id.ToString(),
                CommandType = Enum.GetName(typeof(Enums.commandtype), Enums.commandtype.Delete),
                TransStatement = $"{status} Leave Rule",
                DocumentReferance = id.ToString()
            });

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return Redirect("error/404");

            var leaveRule = await _mediator.Send(new GetByLeaveRuleIdQuery() { LeaveRuleId = (int)id });
            if (leaveRule == null)
                return Redirect("error/404");

            // Use AutoMapper to map the entity to the view model
            var leaveRuleVM = _mapper.Map<LeaveRuleVM>(leaveRule);

            var ComId = _global.GetCompID();
            var OrgId = _global.GetOrgId();
            ViewBag.EmpTypeId = await _dropdown.EmployeeTypeDropdown(ComId, OrgId, leaveRuleVM.EmpTypeId);
            ViewBag.LeaveTypeId = await _dropdown.LeaveTypeDropdown(ComId, OrgId, leaveRuleVM.LeaveTypeId);

            return View("Create", leaveRuleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var tableData = await _mediator.Send(new GetByLeaveRuleIdQuery { LeaveRuleId = (int)id });
                if (tableData != null)
                {
                    await _mediator.Send(new DeleteLeaveRuleCommand() { LeaveRuleId = Convert.ToInt32(id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} Level", DocumentReferance = id.ToString() });
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException.Message;
                return RedirectToAction("Index");
            }

        }
    }
}