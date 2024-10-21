using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class Recruitment
    {
        public class Job: Audit_OrgWithClient
        {
            public int JobId { get; set; }
            public string JobType { get; set; }
            public string SOCCode { get; set; }
            public int DeptId { get; set; }
            public string JobTitle { get; set; }
            public string Description { get; set; }
        }
    }
}
