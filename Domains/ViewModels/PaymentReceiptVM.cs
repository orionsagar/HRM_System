using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class PaymentReceiptMasterVM
    {
        public int PaymentReceiptMasterId { get; set; }
        public int BillingMasterId { get; set; }
        [DisplayName("Payment Receipt No")]
        public string PRNo { get; set; }
        [DisplayName("Payment Receipt Date")]
        public DateTime BillingDate { get; set; }
        public DateTime PRDate { get; set; }
        [DisplayName("Bill Amount")]
        public decimal BillAmount { get; set; }
        [DisplayName("Receipt Amount")]
        public decimal ReceiptAmount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        [DisplayName("Organisation")]
        public int OrgId { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public int UpdatedBy { get; set; }
        public int TOTALCOUNT { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<PaymentReceiptDetailsVM> ReceiptDetailsVMs { get; set; }

    }
    public class PaymentReceiptDetailsVM
    {
        public int PRDetailsId { get; set; }
        public int PaymentReceiptMasterId { get; set; }
        public string ServiceName { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}
