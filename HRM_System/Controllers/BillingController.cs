using Application.Tasks.Commands.CBilling;
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
    public class BillingController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly NumberGeneration _ng;
        private readonly CommonDropdown _common;

        public BillingController(IMediator mediator, IGlobalHelper global, NumberGeneration ng, CommonDropdown common)
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
                var bill = new List<BillingMasterVM>();

                bill = await _mediator.Send(new SP_Dt_BillListQuery() { DisplayLength = Convert.ToInt32(length), DisplayStart = Convert.ToInt32(start), SortCol = Convert.ToInt32(ordercolumn), SortDir = orderdirection, Search = search, OrgId = OrgId });
                if (bill.Count != 0)
                {
                    if (bill.FirstOrDefault().TOTALCOUNT != 0)
                    {
                        totalrecord = bill.FirstOrDefault().TOTALCOUNT;
                    }
                    else
                    {
                        totalrecord = 0;
                    }
                }
                return Json(new { draw = draw, recordsFiltered = totalrecord, recordsTotal = totalrecord, data = bill });
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
            ViewBag.OrgId = await _common.OrganisationDropdown(OrgId,ClientId);
            ViewBag.billno = await _ng.GenarateBillNo(19, DateTime.Now, true, true);
            ViewBag.PackageId = await _common.PackageDropdownForAppendGrid();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BillingMasterVM billing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    decimal totalamount = 0;
                    string[] uniqueIndexes = Request.Form["tblAppendGrid_rowOrder"].ToString().Split(',');
                    List<BillingDetailsVM> billingDetails = new List<BillingDetailsVM>();
                    for (int row = 0; row < uniqueIndexes.Length; row++)
                    {
                        BillingDetailsVM detail = new BillingDetailsVM();
                        string packageId = Request.Form["tblAppendGrid_packageId_" + uniqueIndexes[row]];
                        string amount = Request.Form["tblAppendGrid_amount_" + uniqueIndexes[row]];
                        string note = Request.Form["tblAppendGrid_note_" + uniqueIndexes[row]];
                        string billingDetailsId = Request.Form["tblAppendGrid_billingDetailsId_" + uniqueIndexes[row]];
                        string billingMasterId = Request.Form["tblAppendGrid_billingMasterId_" + uniqueIndexes[row]];


                        detail.PackageId = string.IsNullOrEmpty(packageId) ? 0 : Convert.ToInt32(packageId);
                        detail.Amount = string.IsNullOrEmpty(amount) ? 0 : Convert.ToDecimal(amount);
                        detail.Note = string.IsNullOrEmpty(note) ? "" : note;
                        detail.BillingDetailsId = string.IsNullOrEmpty(billingDetailsId) ? 0 : Convert.ToInt32(billingDetailsId);
                        detail.BillingMasterId = string.IsNullOrEmpty(billingMasterId) ? 0 : Convert.ToInt32(billingMasterId);
                        totalamount += detail.Amount;
                        billingDetails.Add(detail);
                    }
                    billing.BillingDetailsVM = billingDetails;
                    billing.TotalAmount = totalamount;
                    billing.OrgId = billing.OrgId == 0 ? _global.GetOrgId() : billing.OrgId;
                    await _mediator.Send(new UpsertBillCommand() { BillingMasterVM = billing });
                    return Json(new BLStatus { Data = billing, IsError = false, Message = "Data Saved Successfully", StatusCode = "200" });
                }
                catch (Exception ex)
                {
                    return Json(new BLStatus { Data = billing, IsError = true, Message = ex.Message, StatusCode = "500" });
                    throw;
                }
            }
            else
            {
                return Json(new BLStatus { Data = billing, IsError = true, Message = "Model State not valid", StatusCode = "500" });
            }                     
        }



        public async Task<IActionResult> GetBillNoByOrgId(int OrgId, string BillingDate)
        {
            var date = _global.DateConvertionCSharp(BillingDate);
            var billno = await _ng.GenarateBillNo(OrgId, date, true, true);
            return Json(billno);
        }

        public async Task<JsonResult> GetBillInfoByMasterId(int BillingMasterId)
        {//getbillinfobymasterid
            var bill = await _mediator.Send(new GetBillByIdQuery { BillingMasterId = BillingMasterId });
            var payment = await _mediator.Send(new GetPaymentReceiptByBillingMasterIdQuery { BillingMasterId = BillingMasterId });
            var payamount = payment.Sum(s => s.ReceiptAmount);
            return Json(new { bill, payamount });
        }
    }
}
