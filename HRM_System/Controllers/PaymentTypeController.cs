using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Handlers.HPaymentType;
using Application.Tasks.Queries.QPaymentType;
using Domains.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;

namespace UKHRM.Controllers
{
    public class PaymentTypeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;

        public PaymentTypeController(IMediator mediator, IGlobalHelper global)
        {
            _mediator = mediator;
            _global = global;
        }

        public async Task<IActionResult> Index()
        {
            var orgid = _global.GetOrgId();  // Get the OrgId
            var result = await _mediator.Send(new GetAllPaymentTypeQuery { OrgId = orgid });
            return View(result);
        }
        public IActionResult Create()
        {
            ViewBag.Action = "Add";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentType paymentType)
        {
            var effectedid = await _mediator.Send(new UpsertPaymentTypeCommand { PaymentType = paymentType });
            var Id = 0;
            var status = "";
            if (paymentType.PaymentTypeId > 0)
            {
                Id = paymentType.PaymentTypeId;
                status = "Update";
            }
            else
            {
                Id = effectedid;
                status = "Creade";
            }
            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = Id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = $"{status} Payment Type", DocumentReferance = Id.ToString() });

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return Redirect("error/404");
            var data = await _mediator.Send(new GetByPaymentTypeIdQuery() { PaymentTypeId = (int)id });
            if (data == null)
                return Redirect("error/404");
            return View("Create", data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                var tableData = await _mediator.Send(new GetByPaymentTypeIdQuery { PaymentTypeId = (int)Id });
                if (tableData != null)
                {
                    await _mediator.Send(new DeletePaymentTypeCommand() { PaymentTypeId = Convert.ToInt32(Id) });

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
