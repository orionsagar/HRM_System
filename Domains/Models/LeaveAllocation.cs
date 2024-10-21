using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class LeaveAllocation : Audit_OrgWithClient
    {
        public int LeaveAllocationId { get; set; }
        public int EmpTypeId { get; set; }
        public int EmpId { get; set; }
        public int Year { get; set; }

    }
}
