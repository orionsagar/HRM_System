using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class LeaveRule : Audit_OrgWithClient
    {
        public int LeaveRuleId { get; set; }
        public int EmpTypeId { get; set; }
        public int LeaveTypeId { get; set; }
        public double MinimumLeave { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}
