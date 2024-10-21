using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class SystemLock : Audit_Company
    {
        [Key]
        public int LockId { get; set; }
        public string Type { get; set; }
        public DateTime UpToDate { get; set; }
    }

 

   

}
