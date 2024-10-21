using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class LeaveAllocationVM : Audit_OrgWithClient
    {
        public int LeaveAllocationId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int Year { get; set; }

    }
}
