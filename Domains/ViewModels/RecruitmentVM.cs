using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class RecruitmentVM
    {
        public class JobVM : Audit_OrgWithClient
        {
            public int JobId { get; set; }
            public string JobType { get; set; }
            public string SOCCode { get; set; }
            public int DeptId { get; set; }
            public string JobTitle { get; set; }
            public string DeptName { get; set; }
            public string Description { get; set; }
            public int ROWNUM { get; set; }
            public int TOTALCOUNT { get; set; }
            public int CompId { get; set; }
        }
    }
}
