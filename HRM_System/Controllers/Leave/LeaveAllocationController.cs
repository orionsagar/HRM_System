
//using Application.Tasks.Handlers.HLeaveAllocation;
//using Domains.Models;
//using Domains.ViewModels;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using UKHRM.Helper;

//namespace UKHRM.Controllers.Leave
//{
//    public class LeaveAllocationController : Controller
//    {
//        private readonly IMediator _mediator;
//        private readonly CommonDropdown _dropdown;
//        private readonly IGlobalHelper _global;
//        public LeaveAllocationController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global)
//        {
//            _mediator = mediator;
//            _dropdown = dropdown;
//            _global = global;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var param = new DataTableParamVM
//            {
//                OrgId = _global.GetOrgId()
//            };
//            var result = await _mediator.Send(new GetAllLeaveAllowcationQuery { Param = param });
//            return View(result);
//        }

//        public async Task<IActionResult> Create()
//        {
//            var ComId = _global.GetCompID();
//            var OrgId = _global.GetOrgId();
//            ViewBag.EmpTypeId = await _dropdown.EmployeeTypeDropdown(ComId, OrgId, 0);
//            ViewBag.EmpId = await _dropdown.EmployeeDropdown(ComId, OrgId, 0);
//            return View();
//        }




//    }
//}


using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Handlers.HLeaveAllocation;
using Application.Tasks.Handlers.HReaveRule;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;

public class LeaveAllocationController : Controller
{
    private readonly IMediator _mediator;
    private readonly CommonDropdown _dropdown;
    private readonly IGlobalHelper _global;
    private readonly IMapper _mapper;

    public LeaveAllocationController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global, IMapper mapper)
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
        var result = await _mediator.Send(new GetAllLeaveAllowcationQuery { Param = param });
        return View(result);
    }

    public async Task<IActionResult> Create()
    {
        var ComId = _global.GetCompID();
        var OrgId = _global.GetOrgId();
        ViewBag.EmpTypeId = await _dropdown.EmployeeTypeDropdown(ComId, OrgId, 0);
        ViewBag.EmpId = await _dropdown.EmployeeDropdown(ComId, OrgId, 0);
        ViewBag.Action = "Add";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeaveAllocationVM leaveAllocationVM)
    {
        if (ModelState.IsValid)
        {
            // Ensure the necessary values are set
            leaveAllocationVM.OrgId = _global.GetOrgId();
            leaveAllocationVM.ClientId = _global.GetClientId();
            leaveAllocationVM.AddedBy = _global.GetUserID();
            leaveAllocationVM.AddedDate = DateTime.Now;

            if (leaveAllocationVM.LeaveAllocationId > 0)
            {
                leaveAllocationVM.UpdatedBy = _global.GetUserID();
                leaveAllocationVM.UpdatedDate = DateTime.Now;
            }

            // Send the upsert command
            var result = await _mediator.Send(new UpsertLeaveAllocationCommand { LeaveAllocationVM = leaveAllocationVM });

            // Check the result and redirect or provide feedback
            if (result > 0)
            {
                // Successful create or update
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the error case
                ModelState.AddModelError("", "An error occurred while saving the leave allocation.");
            }
        }

        // Reload dropdowns in case of an error
        var ComId = _global.GetCompID();
        var OrgId = _global.GetOrgId();
        ViewBag.EmpTypeId = await _dropdown.EmployeeTypeDropdown(ComId, OrgId, 0);
        ViewBag.EmpId = await _dropdown.EmployeeDropdown(ComId, OrgId, 0);

        return View(leaveAllocationVM);
    }


    public async Task<IActionResult> Edit(int? id)
    {

        if (id == null)
            return Redirect("error/404");

        var leaveAllocation = await _mediator.Send(new GetByLeaveAllocationIdQuery() { LeaveAllocationId = (int)id });
        if (leaveAllocation == null)
            return Redirect("error/404");

        // Use AutoMapper to map the entity to the view model
        //var LeaveAllocationVM = _mapper.Map<LeaveAllocationVM>(leaveRule);

        var ComId = _global.GetCompID();
        var OrgId = _global.GetOrgId();
        ViewBag.EmpTypeId = await _dropdown.EmployeeTypeDropdown(ComId, OrgId, leaveAllocation.EmpTypeId);
        ViewBag.EmpId = await _dropdown.EmployeeDropdown(ComId, OrgId, leaveAllocation.EmpId);

        return View("Create", leaveAllocation);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        try
        {
            var tableData = await _mediator.Send(new GetByLeaveAllocationIdQuery { LeaveAllocationId = (int)id });
            if (tableData != null)
            {
                await _mediator.Send(new DeleteLeaveAllocationCommand() {  LeaveAllocationId = Convert.ToInt32(id) });

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

