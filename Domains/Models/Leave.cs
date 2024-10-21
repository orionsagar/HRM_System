using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
	public class LeaveType : Audit_Company
	{
		public int LeaveTypeId { get; set; }
        public int? OrgId { get; set; }
        public string LTypeName { get; set; }
        public string SortCode { get; set; }
        public int MaxLength { get; set; }
		public string Remark { get; set; }
	}

	public class LeavePayMethod : Audit_Company
	{
		public int LeavePayMethodId { get; set; }

        public int? OrgId { get; set; }
        public string PayMethodName { get; set; }
		public string Remark { get; set; }
	}

	public class LeaveDays : Audit_Company
	{
		public int LeaveDaysId { get; set; }
		public int EmpId { get; set; }
		[ForeignKey("EmpId")]
		public virtual Employee Employee { get; set; }
		public DateTime LeaveDate { get; set; }
		public string Remarks { get; set; }
		public int RefNo { get; set; }
        public int? OrgId { get; set; }
    }
	
	public class FiscalYear : Audit_Company
	{
		public int FiscalYearId { get; set; }
		public int? OrgId { get; set; }
		public string Title { get; set; }
		public DateTime startdate { get; set; }
		public DateTime enddate { get; set; }
	}

	public class LeaveSetup : Audit_Company
	{
		public int LeaveSetupId { get; set; }
		public int EmpId { get; set; }
        [NotMapped]
		public bool empAllCheck { get; set; }
        [ForeignKey("EmpId")]
		public virtual Employee Employee { get; set; }
		public int LeaveTypeId { get; set; }
		public virtual LeaveType LeaveType { get; set; }		
		public int LeaveDays { get; set; }
		public string Remark { get; set; }
		public int FiscalYearId { get; set; }
        public int? OrgId { get; set; }
        public virtual FiscalYear FiscalYear { get; set; }
	}

	public class ShortLeaveSetup : Audit_Company
	{
		public int ShortLeaveSetupId { get; set; }
		public int EmpId { get; set; }
        [NotMapped]
		public bool empAllCheck { get; set; }
        [ForeignKey("EmpId")]
		public virtual Employee Employee { get; set; }		
		public DateTime EffectiveFrom { get; set; }
		public DateTime EffectiveTo { get; set; }
		public int SLeaveDays { get; set; }
		public string Remark { get; set; }
	}

	public class ShortLeaveAssign : Audit_Company
	{
		public int ShortLeaveAssignId { get; set; }
		public int EmpId { get; set; }       
        [ForeignKey("EmpId")]
		public virtual Employee Employee { get; set; }		
		public DateTime EffectiveDate { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }		
		public string Remark { get; set; }
	}

	

	public class LeaveTransaction : Audit_Company
	{
		public int LeaveTransactionId { get; set; }
		public int EmpId { get; set; }
		[ForeignKey("EmpId")]
		public virtual Employee Employee { get; set; }
		public int LeaveTypeId { get; set; }        
		public virtual LeaveType LeaveType { get; set; }
		[NotMapped]
		public string LeaveTypeName { get; set; }
		public DateTime? GrantDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        [NotMapped]
		public DateTime DOJ { get; set; }
		public int TotalDays { get; set; }
		public int OrgId { get; set; }
		public int LeavePayMethodId { get; set; }
		public virtual LeavePayMethod LeavePayMethod { get; set; }
		public string Remark { get; set; }
	}

	public class EarnLeave : Audit_Company
	{
        public int EarnLeaveId { get; set; }
        public int FiscalYearId { get; set; }
		[ForeignKey("FiscalYearId")]
		public virtual FiscalYear FiscalYear { get; set; }
		public int EmpId { get; set; }
		[ForeignKey("EmpId")]
		public virtual Employee Employee { get; set; }
        public int OpeningBalance { get; set; }
        public int LeaveTransaction { get; set; }
        public float TotalEarnLeave { get; set; }
        public float? Month1 { get; set; }
        public int OldFiscalYearId { get; set; }
        public int? OrgId { get; set; }
    }
}
