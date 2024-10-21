using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class PaymentMoad : Audit_OrgWithClient
    {
        public int PaymentMoadId { get; set; }
        public string PaymentModeName { get; set; }
    }
}
