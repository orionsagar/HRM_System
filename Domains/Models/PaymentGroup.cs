using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class PaymentGroup : Audit_OrgWithClient
    {
        [Key]
        public int PayGroupId { get; set; }
        public string PayGroup { get; set; }
        public string Status { get; set; }

    }
}
