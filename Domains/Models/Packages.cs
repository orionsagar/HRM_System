using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class Packages
    {
        [Key]
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int MaxEmp { get; set; }
        public double RatePerMonth { get; set; }
        public int CurrID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int MaxAdmin { get; set; }
        public int MaxUser { get; set; }
    }
}
