using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class PayDetails : Audit_OrgWithClient
    {
        public int PayDetailsId { get; set; }

        [DisplayName("Payment Type")]
        public int PaymentTypeId { get; set; }

        [DisplayName("Annual Pay")]
        public int AnnualPayId { get; set; }

        [DisplayName("Payment Mode")]
        public int PaymentModeId { get; set; }

        [DisplayName("Pay Group")]
        public int PayGroupId { get; set; }

        [DisplayName("Basic / Daily Wedges")]
        public double BasicWedges { get; set; }

        [DisplayName("Min. Working Hour")]
        public double MinWorkingHour { get; set; }
        public double Rate { get; set; }

        [DisplayName("Tax Code")]
        public int TaxId { get; set; }

        [DisplayName("Tax Reference")]
        public string TaxReference { get; set; }

        [DisplayName("Tax Percentage")]
        public double TaxPercentage { get; set; }

        [DisplayName("Bank Name")]
        public int BankId { get; set; }

        [DisplayName("Branch Name")]
        public string BranchName { get; set; }

        [DisplayName("Account No")]
        public string AccountNo { get; set; }

        [DisplayName("Sort Code")]
        public string SortCode { get; set; }

        [DisplayName("Payment Currency")]
        public string PaymentCurrency { get; set; }
    }
}
