using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class EmpLeaveFilter : EmployeeFilterVM
    {
        public bool IsNoLeave { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class EarnLeaveVM
    {
        public int EarnLeaveId { get; set; }
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public int FiscalYearId { get; set; }
        public int OldFiscalYearId { get; set; }
        public int EmpId { get; set; }
        public int DesigId { get; set; }
        public string DesignationName { get; set; }
        public string SectionName { get; set; }
        public int SectId { get; set; }
        public int FloorID { get; set; }
        public int LineID { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public float OpeningBalance { get; set; }
        public int CurrentEarnLeave { get; set; }
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public string DepartmentName { get; set; }
        public int UserId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class LeaveDetailsVM
    {
        public int EmpId { get; set; }
        public int DeptId { get; set; }
        public int SectId { get; set; }
        public int DesigId { get; set; }
        public int FiscalYearId { get; set; }
        public int LeaveTypeId { get; set; }
        public int LeaveDays { get; set; }
        public int TotalSpent { get; set; }
        public int BalanceDays { get; set; }
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string DesignationName { get; set; }
        public string LTypeName { get; set; }
    }
    public class EmployeeLeaveSummaryVM
    {
        public int EmpId { get; set; }
        public string CardNo { get; set; }
        public string Name { get; set; }
        public string DesignationName { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string JoiningDate { get; set; }
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
        public int TotalEnjoyedLeave { get; set; }
        public int Balance { get; set; }
    }

    public class LeaveTransactionVM
    {
        public int LeaveTransactionId { get; set; }
        public int EmpId { get; set; }
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public int LeaveTypeId { get; set; }
        public string LTypeName { get; set; }
        public DateTime? GrantDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DOJ { get; set; }
        public int TotalDays { get; set; }
        public int ROWNUM { get; set; }
        public int TOTALCOUNT { get; set; }
        public int LeavePayMethodId { get; set; }
        public string PayMethodName { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }
    public class LeaveReport
    {
        public int EmpId { get; set; }
        public int SectId { get; set; }
        public int DesigId { get; set; }
        public int DeptId { get; set; }
        public string CardNo { get; set; }
        public string EmpName { get; set; }
        public int FiscalYearId { get; set; }
        public DateTime Month { get; set; }
    }

    public class LeaveTypeVM
    {
        //public int LeaveSetupId { get; set; }      
        public int LeaveTypeId { get; set; }
        public int LeaveDays { get; set; }
        public string Remark { get; set; }
        public int FiscalYearId { get; set; }
       
    }
    public class LeaveSutupVM
    {
        public List<LeaveTypeVM> LeaveTypes { get; set; }
        public int[] EmpIds { get; set; }      
        public int FiscalYearId { get; set; }
        public int? OrgId { get; set; }
       
    }

    public class ShortLeaveAssignFilter : EmployeeFilterVM
    {
        public DateTime FromDate_Filter { get; set; }
        public DateTime ToDate_Filter { get; set; }        
    }

    public class ShortLeaveAssignEmpListVM : EmployeeVM
    {
        public int LeaveDays { get; set; }
        public int LeaveEnjoyCurrentMonth { get; set; }
        public int ShortLeaveAssignID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Remark { get; set; }
    }


    public class ShortLeaveAssignVM : Audit_Company
    {
        public int ShortLeaveAssignID { get; set; }
        public int EmpId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Remark { get; set; }
    }
}
