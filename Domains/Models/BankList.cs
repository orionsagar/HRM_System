using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class BankLists : Audit_OrgWithClient
    {
        [Key]
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankSortCode { get; set; }
        public string Status { get; set; }

        public string CreatedBy { get; set; }

    }
}