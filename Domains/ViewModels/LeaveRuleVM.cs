using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class LeaveRuleVM : Audit_OrgWithClient
    {
        public int LeaveRuleId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public double MinimumLeave { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
    }
}
