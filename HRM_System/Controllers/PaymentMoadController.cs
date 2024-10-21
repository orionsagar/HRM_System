using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Handlers.HPaymentMoad;
using Domains.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;

namespace UKHRM.Controllers
{
    //[Authorize]
    public class PaymentMoadController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        public PaymentMoadController(IMediator mediator,IGlobalHelper global)
        {
            _mediator = mediator;
            _global = global;
        }

        public async Task<IActionResult> Index()
        {
            var orgid = _global.GetOrgId();  // Get the OrgId
            var result = await _mediator.Send(new GetAllPaymentMoadQuery { OrgId = orgid });
            return View(result);
        }
        public IActionResult Create()
        {
            ViewBag.Action = "Add";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentMoad paymentMoad)
        {
            var effectedid = await _mediator.Send(new UpsertPaymentMoadCommand { PaymentMoad = paymentMoad});
            var Id = 0;
            var status = "";
            if(paymentMoad.PaymentMoadId > 0)
            {
                Id = paymentMoad.PaymentMoadId;
                status = "Update";
            }
            else
            {
                Id = effectedid;
                status = "Creade";
            }
            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{status} Payment Moad", DocumentReferance = Id.ToString() });

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return Redirect("error/404");
            var data = await _mediator.Send(new GetByPaymentMoadIdQuery() {  PaymentMoadId = (int)id });
            if (data == null)
                return Redirect("error/404");
            return View("Create",data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                var tableData = await _mediator.Send(new GetByPaymentMoadIdQuery { PaymentMoadId = (int)Id });
                if (tableData != null)
                {
                    await _mediator.Send(new DeletePaymentMoadCommand() { PaymentMoadId = Convert.ToInt32(Id) });

                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{Enums.commandtype.Delete} Level", DocumentReferance = Id.ToString() });
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
