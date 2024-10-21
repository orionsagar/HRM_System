using Application.Tasks.Commands.CBankList;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Commands.HPaymentGroup;
using Application.Tasks.Handlers.HLeaveAllocation;
using Application.Tasks.Handlers.HPaymentGroup;
using Application.Tasks.Handlers.HReaveRule;
using Application.Tasks.Queries.HPaymentGroup;
using Application.Tasks.Queries.QBankList;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using UKHRM.Helper;
using UKHRM.Helpers;
using Utility;

public class PaymentGroupController : Controller
{
    private readonly IMediator _mediator;
    private readonly CommonDropdown _dropdown;
    private readonly IGlobalHelper _global;
    private readonly IMapper _mapper;

    public PaymentGroupController(IMediator mediator, CommonDropdown dropdown, IGlobalHelper global, IMapper mapper)
    {
        _mediator = mediator;
        _dropdown = dropdown;
        _global = global;
        _mapper = mapper;
    }


    public async Task<IActionResult> Index()
    {
        #region Access
        ViewBag.Action = "Add";
        var roleid = _global.GetRoleID();
        var controller = RouteData.Values["controller"];
        var action = RouteData.Values["action"];
        var url = $"{controller}/{action}";
        ViewBag.IsView = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "View");
        ViewBag.IsDelete = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Delete");
        ViewBag.IsEdit = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Edit");
        ViewBag.IsAdd = await ClsUserAccess.PageGetRolewiseAccess(url, Convert.ToInt32(roleid), "Save");
        #endregion

        var ClientId = _global.GetClientId();
        var orgid = _global.GetOrgId();
        ViewBag.OrgId = await _dropdown.OrganisationDropdown(orgid, ClientId);

        return View();
    }

    public async Task<IActionResult> loaddata()
    {
        var draw = Request.Form["draw"].FirstOrDefault();
        var start = Request.Form["start"].FirstOrDefault();
        var length = Request.Form["length"].FirstOrDefault();
        var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
        var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
        var search = Request.Form["search[value]"].FirstOrDefault();
        var orgid = _global.GetOrgId();  // Get the OrgId
        var totalrecord = 0;

        var data = await _mediator.Send(new GetAllPaymentGroupQuery { OrgId = orgid });


        if (data.Count != 0)
        {
            if (data != null)
            {
                totalrecord = data.Count;
            }
            else
            {
                totalrecord = 0;
            }
        }
        return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = data });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> create(PaymentGroup paymentGroup)
    {
        try
        {
            if (paymentGroup.PayGroupId > 0)
            {

                await _mediator.Send(new UpdatePaymentGroupCommand() { PaymentGroup = paymentGroup });

                var json = JsonConvert.SerializeObject(paymentGroup);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = paymentGroup.PayGroupId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} PaymentGroup", DocumentReferance = json });

                return Json(new BLStatus { Message = "Data Update Successfully." });
            }
            else
            {

                await _mediator.Send(new CreatePaymentGroupCommand() { PaymentGroup = paymentGroup });

                var json = JsonConvert.SerializeObject(paymentGroup);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = paymentGroup.PayGroupId.ToString(), CommandType = Enums.commandtype.Update.ToString(), TransStatement = $"{Enums.commandtype.Update} PaymentGroup", DocumentReferance = json });

                return Json(new BLStatus { Message = "Data Save Successfully." });
            }
        }
        catch (Exception)
        {
            return Json(new BLStatus { Message = "Uable To Save/Update Record.", IsError = true });
        }
    }


    public async Task<IActionResult> Edit(int id)
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
        ViewBag.Action = "Edit";
        var data = await _mediator.Send(new GetByPaymentGroupIdQuery() { PayGroupId = id });
        return View("Index", data);
    }


    public async Task<IActionResult> Delete(int id)
    {
        try
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
            var data = await _mediator.Send(new GetByPaymentGroupIdQuery() { PayGroupId = id });
            if (data != null)
            {
                ViewBag.Action = "Delete";
                await _mediator.Send(new DeletePaymentGroupCommand() { PayroupId = id });

                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enums.commandtype.Delete.ToString(), TransStatement = $"{Enums.commandtype.Delete} BankList", DocumentReferance = id.ToString() });
            }
            return Json(new BLStatus { Message = "Delete Data Successfully" });
        }
        catch (Exception)
        {
            return Json(new BLStatus { Message = "Uable To Delete Record.", IsError = true });
        }



    }

}


