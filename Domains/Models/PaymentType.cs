using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class PaymentType : Audit_OrgWithClient
    {
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public double WorkingHour { get; set; }
        public double Rate { get; set; }
    }
}
