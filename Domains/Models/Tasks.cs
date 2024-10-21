using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class Tasks : Audit_OrgWithClient
    {
        [Key]
        public int TaskId { get; set; }
        public string TaskNo { get; set; }
        public string TaskTitle { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDate { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
    }
}
