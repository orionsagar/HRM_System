using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class PaymentReceiptMaster
    {
        [Key]
        public int PaymentReceiptMasterId { get; set; }
        public int BillingMasterId { get; set; }
        public string PRNo { get; set; }
        public DateTime PRDate { get; set; }
        public decimal BillAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int OrgId { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<PaymentReceiptDetails> PaymentReceiptDetails { get; set; }

    }
    public class PaymentReceiptDetails
    {
        [Key]
        public int PRDetailsId { get; set; }
        public int PaymentReceiptMasterId { get; set; }
        public string ServiceName { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}
