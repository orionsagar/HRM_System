using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{

    public class Deduction : Audit_Company
    {
        [Key]
        public int DeductionID { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }
        public string DeductionName { get; set; }
        public double Qty { get; set; }
        public double CostPerUnit { get; set; }
        public double SelfContribution { get; set; }
        public decimal Amount { get; set; }
        public DateTime ApplyDate { get; set; }
        //public int CompanyID { get; set; }
        //public int UserID { get; set; }
        //public DateTime ModifiedDate { get; set; }
        public double ServCharge { get; set; }
        public string Remarks { get; set; }
    }

    public class SnacksAllowance : Audit_Company
    {
        [Key]
        public int SnacksAllowanceID { get; set; }
        [DisplayName("Designation")]
        public int? OrgId { get; set; }
        public int DesigId { get; set; }
        [ForeignKey("DesigId")]
        public virtual Designation Designation { get; set; }
        public DateTime EffectDate { get; set; }
        public bool IsFixed { get; set; }
        public double SnacksAmt { get; set; }
        public double CompContribution { get; set; }
        public double SelfContribution { get; set; }

    }

    public class OtherAllowance : Audit_Company
    {
        [Key]
        public int OtherAllowanceID { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }

        public string AllowanceName { get; set; }
        public bool IsFixed { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Rate { get; set; }
        public int CalcRuleID { get; set; }
        public int? OrgId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ApplyDate { get; set; }
        public string Remarks { get; set; }
        //public int CompanyID { get; set; }
        //public int UserID { get; set; }
        //public DateTime ModifiedDate { get; set; }
    }

    public class AttendanceBonus : Audit_Company
    {
        [Key]
        public int AttendanceBonusID { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }
        public bool IsFixed { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Rate { get; set; }
        public int RuleID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ApplyDate { get; set; }
        public string Remarks { get; set; }

    }

    public class PerformanceBonus : Audit_Company
    {
        public int PerformanceBonusID { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }
        public bool IsFixed { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Rate { get; set; }
        public int CalcRuleID { get; set; }
        public decimal Amount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ApplyDate { get; set; }
        public string Remarks { get; set; }
    }

    public class FestivalBonus : Audit_Company
    {
        public int FestivalBonusID { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Employee { get; set; }
        public bool IsFixed { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Rate { get; set; }
        public int CalcRuleID { get; set; }
        public decimal Amount { get; set; }
        public byte ByServiceAge { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ApplyDate { get; set; }
        public string Remarks { get; set; }
        //[ForeignKey("FestivalTypeId")]
        public int FestivalTypeId { get; set; }
        //public virtual FestivalType FestivalType { get; set; }

    }



}
