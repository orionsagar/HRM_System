using Domains.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public  class FestivalBonusSetup : Audit_Company
    {
        [Key]
        public int BonusId { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("FestivalTypeId")]
        public int FestivalTypeId { get; set; }

        public bool IsFixed { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Rate { get; set; }
        public int CalcRuleID { get; set; }
        public string Remarks { get; set; }
    
        public virtual FestivalType FestivalType { get; set; }
    }
    public class FestivalType
    {
        [Key]
        public int FestivalTypeId { get; set; }
        public string FestivalName { get; set; }
    
    }
}
