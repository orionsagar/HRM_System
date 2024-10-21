using Application.Tasks.Commands.CBilling;
using Application.Tasks.Commands.CPaymentReceipt;
using Application.Tasks.Queries.QBill;
using Application.Tasks.Queries.QPaymentReceipt;
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
    public class PaymentReceiptController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly NumberGeneration _ng;
        private readonly CommonDropdown _common;

        public PaymentReceiptController(IMediator mediator, IGlobalHelper global, NumberGeneration ng, CommonDropdown common)
        {
            _mediator = mediator;
            _global = global;
            _ng = ng;
            _common = common;
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
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var ordercolumn = Request.Form["order[0][column]"].FirstOrDefault();
                var orderdirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var search = Request.Form["search[value]"].FirstOrDefault();
                var totalrecord = 0;
                var UserId = DataEncryption.DecryptString(Request.Cookies["UserID"]);
                //var Role = DataEncryption.DecryptString(Request.Cookies["Role"]);
                var OrgId = _global.GetOrgId();
                var payments = new List<PaymentReceiptMasterVM>();

                payments = await _mediator.Send(new SP_Dt_PaymentReceiptListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, OrgId = OrgId });
                if (payments.Count != 0)
                {
                    if (payments.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = payments.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = payments });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Create()
        {
            var OrgId = _global.GetOrgId();
            var ClientId = _global.GetClientId();
            ViewBag.OrgId = await _common.OrganisationDropdown(OrgId, ClientId);
            ViewBag.BillNo = await _common.BillNoDropdown(OrgId);
            ViewBag.PPNo= await _ng.GenaratePRNo(19, DateTime.Now, true, true);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PaymentReceiptMasterVM payment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    decimal totalamount = 0;
                    string[] uniqueIndexes = Request.Form["tblAppendGrid_rowOrder"].ToString().Split(',');
                    List<PaymentReceiptDetailsVM> Details = new List<PaymentReceiptDetailsVM>();
                    for (int row = 0; row < uniqueIndexes.Length; row++)
                    {
                        PaymentReceiptDetailsVM detail = new PaymentReceiptDetailsVM();
                        string serviceName = Request.Form["tblAppendGrid_serviceName_" + uniqueIndexes[row]];
                        string amount = Request.Form["tblAppendGrid_amount_" + uniqueIndexes[row]];
                        string note = Request.Form["tblAppendGrid_note_" + uniqueIndexes[row]];
                        string billingDetailsId = Request.Form["tblAppendGrid_prDetailsId_" + uniqueIndexes[row]];
                        string billingMasterId = Request.Form["tblAppendGrid_paymentReceiptMasterId_" + uniqueIndexes[row]];


                        detail.ServiceName = string.IsNullOrEmpty(serviceName) ? "" : serviceName;
                        detail.Amount = string.IsNullOrEmpty(amount) ? 0 : Convert.ToDecimal(amount);
                        detail.Note = string.IsNullOrEmpty(note) ? "" : note;
                        detail.PRDetailsId = string.IsNullOrEmpty(billingDetailsId) ? 0 : Convert.ToInt32(billingDetailsId);
                        detail.PaymentReceiptMasterId = string.IsNullOrEmpty(billingMasterId) ? 0 : Convert.ToInt32(billingMasterId);
                        totalamount += detail.Amount;
                        Details.Add(detail);
                    }
                    payment.ReceiptDetailsVMs = Details;
                    payment.ReceiptAmount += totalamount;
                    payment.OrgId = payment.OrgId == 0 ? _global.GetOrgId() : payment.OrgId;
                    await _mediator.Send(new UpsertPaymentReceiptCommand() { ReceiptMasterVM = payment });
                    return Json(new BLStatus { Data = payment, IsError = false, Message = "Data Saved Successfully", StatusCode = "200" });
                }
                catch (Exception ex)
                {
                    return Json(new BLStatus { Data = payment, IsError = true, Message = ex.Message, StatusCode = "500" });
                    throw;
                }
            }
            else
            {
                return Json(new BLStatus { Data = payment, IsError = true, Message = "Model State not valid", StatusCode = "500" });
            }                     
        }



        public async Task<IActionResult> GetPRNoByOrgId(int OrgId, string PRDate)
        {
            var date = _global.DateConvertionCSharp(PRDate);
            var prno = await _ng.GenaratePRNo(OrgId, date, true, true);
            return Json(prno);
        }
    }
}
