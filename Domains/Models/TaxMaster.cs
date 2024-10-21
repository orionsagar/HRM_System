using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class TaxMaster : Audit_OrgWithClient
    {
        [Key]
        public int TaxId { get; set; } 
        public string TaxCode { get; set; } 
        public int TaxPercent { get; set; } 
        public string TaxReference { get; set; } 
    }
}
