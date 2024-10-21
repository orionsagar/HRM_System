using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class PayDetailsVM : Audit_OrgWithClient
    {
        public int PayDetailsId { get; set; }
        public int PaymentTypeId { get; set; }
        public int AnnualPayId { get; set; }
        public int PaymentModeId { get; set; }
        public int PayGroupId { get; set; }
        public double BasicWedges { get; set; }
        public double MinWorkingHour { get; set; }
        public double Rate { get; set; }
        public int TaxId { get; set; }
        public string TaxReference { get; set; }
        public double TaxPercentage { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string SortCode { get; set; }
        public string PaymentCurrency { get; set; }
    }
}
