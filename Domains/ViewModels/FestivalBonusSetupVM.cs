using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class FestivalBonusSetupVM : Audit_Company
    {
        [Key]
        public int BonusId { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }
        public int FestivalTypeId { get; set; }
        public string Remarks { get; set; }
        
        public virtual ICollection<FestivalTypeVM> FestivalTypeVM { get; set; }
    }
    public class FestivalTypeVM
    {
        [Key]
        public int FestivalTypeId { get; set; }
        public int FestivalName { get; set; }
        public int CalcRuleID { get; set; }
        public decimal Rate { get; set; }

        public virtual FestivalBonusSetupVM BonusSetupVM { get; set; }
    }
}
