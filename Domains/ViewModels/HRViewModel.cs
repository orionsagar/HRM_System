using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.ViewModels
{
    public class CompanyViewModel
    {
        [Key]
        public int CompID { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        //[StringLength(3, ErrorMessage = "Name must be 3 characters and first charecter must be 0")]
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        public string ComCode { get; set; }
   
        public string TradeName { get; set; }
        public string RegitrationNumber { get; set; }
        public string OrganizationType { get; set; }
        public string OrganizationSize { get; set; }
        public string SectorName { get; set; }
        public string TradingPeriod { get; set; }
        public string ContactNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string OrganizationMail { get; set; }
        public string RegAddressL1 { get; set; }
        public string RegAddressL2 { get; set; }
        public string RegAddressL3 { get; set; }
        public string RegCity { get; set; }
        public string RegPost { get; set; }
        public string RegCountry { get; set; }
        public bool SameAsReg { get; set; }
        public string TradeAddressL1 { get; set; }
        public string TradeAddressL2 { get; set; }
        public string TradeAddressL3 { get; set; }
        public string TradeCity { get; set; }
        public string TradePost { get; set; }
        public string TradeCountry { get; set; }
        public bool OrganizationPenalty { get; set; }





        [Display(Name = "Comp Type")]
        public string CompType { get; set; }
        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }
        public string Attention { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Hot Number")]
        [RegularExpression(@"^\(?([0-0]{1})\)?[-. ]?([1-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Mobile number(The mobile number must start with 0)")]
        [StringLength(11, ErrorMessage = "mobile number must be 11 characters and first charecter must be 0")]
        public string HotNumber { get; set; }
        [Display(Name = "BKash Number")]
        [RegularExpression(@"^\(?([0-0]{1})\)?[-. ]?([1-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Mobile number(The mobile number must start with 0)")]
        [StringLength(11, ErrorMessage = "mobile number must be 11 characters and first charecter must be 0")]
        public string BKashNumber { get; set; }
        [Required]
        //[RegularExpression(@"^\(?([0-0]{1})\)?[-. ]?([1-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Mobile number(The mobile number must start with 0)")]
        //[StringLength(11, ErrorMessage = "mobile number must be 11 characters and first charecter must be 0")]
        public string Phone1 { get; set; }
        // [RegularExpression(@"^\(?([0-0]{1})\)?[-. ]?([1-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Mobile number(The mobile number must start with 0)")]
        //[RegularExpression(@"/^(\+\d{1,3}[- ]?)?\d{10}$/", ErrorMessage = "Not a valid Mobile number(The mobile number must start with 0)")]
        //[StringLength(11, ErrorMessage = "mobile number must be 11 characters and first charecter must be 0")]
        public string Phone2 { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        [Display(Name = "Business Hour")]
        public string BusinessHour { get; set; }
        public string Website { get; set; }
        public string Country { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string twitter { get; set; }
        [Display(Name = "Google Plus")]
        public string GooglePlus { get; set; }
        [Display(Name = "Logo file")]
        public IFormFile Logofile { get; set; }
        public string Logo { get; set; }
        [Display(Name = "FB Pixel ScriptHeader")]
        public string FBPixelScriptHeader { get; set; }
        [Display(Name = "GA ScriptHeader")]
        public string GAScriptHeader { get; set; }
        public int ROWNUM { get; set; }
        public int TOTALCOUNT { get; set; }
        //public bool InitialSetup { get; set; }
        //public string Package { get; set; }
        //public DateTime ContractStartDate { get; set; }
        //public DateTime ContractEndDate { get; set; }
        //public bool VerifyRequired { get; set; }
        //public string AccountType { get; set; }
        //public string DOPrefix { get; set; }
        //public string InvPrefix { get; set; }


        //public PageHeaderProps pageHeader { get; set; }
    }

    public class DesignationVM
    {
        public int DesigId { get; set; }
        public string DesigName { get; set; }
        public string PSTypeName { get; set; }
        public string NameGeneric { get; set; }
        public int PayScaleId { get; set; }
        public string Description { get; set; }
        public string GradeOrScale { get; set; }
        public int ROWNUM { get; set; }
        public int TOTALCOUNT { get; set; }
    }

    public class AnnualPayVM
    {
        public int AnnualPayId { get; set; }
        public string AnnualPayName { get; set; }
        public string PSGroupName { get; set; }
        public string NameGeneric { get; set; }
        public int PayGroupId { get; set; }
        public string Description { get; set; }
        public string GradeOrScale { get; set; }
        public int ROWNUM { get; set; }
        public int TOTALCOUNT { get; set; }
    }

    public class EmployeeVM
    {
        public int EmpId { get; set; }
        public int EmpHistoryId { get; set; }
        public string CardNo { get; set; }
        public string Name { get; set; }
        public string Photograph { get; set; }
        public string NameGeneric { get; set; }
        public string ProximityNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EmpStatusName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Address { get; set; }
        public string NationalId { get; set; }
        public decimal Salary { get; set; } = 0;
        public string EmpTypeName { get; set; }
        public string SectionName { get; set; }
        public string SectId { get; set; }
        public string DeptId { get; set; }
        public string OrgId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string DateOfBirth { get; set; }
        public string CategoryName { get; set; }
        public string Nationality { get; set; }
        public int GradeOrScale { get; set; }


        public string NINumber { get; set; }
        public string VisaExpiryDate { get; set; }
        public string PassportNo { get; set; }
        public string EUSSDetails { get; set; }
        public string DBSDetails { get; set; }
        public string NationalIDDetails { get; set; }

        public int rowNum { get; set; }
        public int TotalCount { get; set; }
        public int CalcRuleId { get; set; }

        public string ContractFile { get; set; }
    }


    public class EmployeeStatusVM
    {
        public int EmpId { get; set; }
        public int EmpHistoryId { get; set; }
        public int EmpStatusId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }
        public int CompId { get; set; }
        public string UserId { get; set; }
    }


    public class EmpStatusFilter : EmployeeFilterVM
    {
        public int EmpStatusId { get; set; }
    }

    public class EmployeeFilterVM
    {
        public int EmpId_Filter { get; set; }
        [Display(Name = "Category")]
        public int EmpCategoryId_Filter { get; set; }
        [Display(Name = "Designation")]
        public int DesignationId_Filter { get; set; }
        [Display(Name = "Section")]
        public int SectionId_Filter { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId_Filter { get; set; }
        [Display(Name = "Floor")]
        public int FloorId_Filter { get; set; }
        [Display(Name = "Line")]
        public int LineId_Filter { get; set; }
        public string CardNo_Filter { get; set; }
        public int ServiceAgeFrom_Filter { get; set; }
        public int ServiceAgeTo_Filter { get; set; }
        public string EmpName_Filter { get; set; }
        public bool HasCheckBox_Filter { get; set; }
        public string CheckBoxName_Filter { get; set; }
        public List<string> HTML { get; set; }
        public int CompId { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
        public string UserId { get; set; }
    }

    public class PayScaleVM : Audit_Company
    {
        public int PayScaleID { get; set; }
        public int RuleID { get; set; }
        public int OrgId { get; set; }

        public int PayScaleTypeId { get; set; }
        public virtual PayScaleType PayScaleType { get; set; }

        public int GradeOrScale { get; set; }
        public decimal InitialPay { get; set; }
        public int YearOfIncr { get; set; }
        public decimal IncrAmount { get; set; }
        public int EBYearOfIncr { get; set; }
        public decimal EBIncrAmount { get; set; }
        public decimal LastPay { get; set; }
        public bool ByGross { get; set; }
        public SalaryBreakDown SalaryBreakDown { get; set; }

    }
    public class PayScaleWithSalaryBreakDownVM
    {
        public virtual PayScale PayScale { get; set; }
        public virtual SalaryBreakDown SalaryBreakDown { get; set; }
    }

    public class SP_Dt_PayScaleList
    {
        public int PayScaleID { get; set; }
        public int GradeOrScale { get; set; }
        public string PayScaleTypeName { get; set; }
        public string RuleName { get; set; }
        public double Basic { get; set; }
        public double HouseRent { get; set; }
        public double Medical { get; set; }
        public double Conveyance { get; set; }
        public double Food { get; set; }
        public double TotalSalary { get; set; }
        public int YearlyIncrementRate { get; set; }
        public int YearOfIncr { get; set; }
        public double IncrAmount { get; set; }
        public int EBYearOfIncr { get; set; }
        public double EBIncrAmount { get; set; }
        public double LastPay { get; set; }
        public bool ByGross { get; set; }
        public int ROWNUM { get; set; }
        public int TOTALCOUNT { get; set; }

    }
    public class SP_Dt_DepartmentList
    {
        public int DeptId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameGeneric { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int CompId { get; set; }
    }
    public class SP_Dt_CategoryList
    {
        public int EmpCatId { get; set; }
        public string Name { get; set; }
        public string NameGeneric { get; set; }
        public string Description { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int CompId { get; set; }
    }

    public class SP_Dt_EmpTypeList
    {
        public int EmpTypeId { get; set; }
        public string Name { get; set; }
        public string NameGeneric { get; set; }
        public string Description { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int CompId { get; set; }
        public int OrgId { get; set; }
    }
    public class SP_Dt_LevelList
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string SortIndex { get; set; }
        public string Status { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int OrgId { get; set; }
    }

    public class SP_Dt_SectionList
    {
        public int SectId { get; set; }
        public string Name { get; set; }
        public string NameGeneric { get; set; }
        public string Description { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int CompId { get; set; }
    }

    public class SP_Dt_OrganogramList
    {
        public int OrghchyID { get; set; }
        public string EmployeeName { get; set; }
        public string LevelName { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int OrgId { get; set; }
    }

    public class SP_Dt_EmpDocList
    {
        public int EmpDocId { get; set; }
        public string Name { get; set; }
        public string EmpID { get; set; }
        public string CardNo { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string PassportNo { get; set; }
        public string PassportNationality { get; set; }
        public string ExpiryDate { get; set; }
        public string VisaBRPNo { get; set; }
        public string VisaNationality { get; set; }
        public string VisaExpiryDate { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int ClientId { get; set; }
        public int OrgId { get; set; }
    }

    public class SP_Dt_EmpNewDocList
    {
        public int EmpNewDocumentsID { get; set; }
        public string Name { get; set; }
        public string EmpID { get; set; }
        public string CardNo { get; set; }
        public string DocumentType { get; set; }
        public string PostCode { get; set; }
        public string CountryOfResidence { get; set; }
        public string Nationality { get; set; }
        public string? IssuedDate { get; set; }
        public string? ExpiryDate { get; set; }
        public string IssueBy { get; set; }
        public string IsCurrentPassport { get; set; }
        public string EligibleReviewDate { get; set; }
        public string NumberOfDoc { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int ClientId { get; set; }
        public int OrgId { get; set; }
    }

    public class SP_Dt_RTWCList
    {
        public int RightToWorkCheckId { get; set; }
        public string Name { get; set; }
        public string EmpID { get; set; }
        public string CardNo { get; set; }
        public string AddedDate { get; set; }
        public int TOTALCOUNT { get; set; }
        public int ROWNUM { get; set; }
        public int ClientId { get; set; }
        public int OrgId { get; set; }
    }

    public class EmpOTRuleFilter : EmployeeFilterVM
    {
        public bool IsNoOTRule { get; set; }
    }
    public class EmpOTListVM : EmployeeVM
    {
        public string OTType { get; set; }
    }


    public class AdvanceSanctionFilter : EmployeeFilterVM
    {
        public DateTime FromDate_Filter { get; set; }
        public DateTime ToDate_Filter { get; set; }
        public string FindCardNo_Filter { get; set; }
    }

    public class AdvanceSanctionEmpListVM : EmployeeVM
    {
        public int AdvanceID { get; set; }
        public DateTime SanctionDate { get; set; }
        public decimal ApplyAmount { get; set; }
        public decimal SanctionAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string Type { get; set; }
    }

    //public class AdvanceSanctionVM: Audit_Company
    //{
    //    public int AdvanceID { get; set; }
    //    public int EmpId { get; set; }
    //    public decimal ApplyAmount { get; set; }
    //    public decimal SanctionAmount { get; set; }
    //    public decimal DueAmount { get; set; }
    //    public DateTime SanctionDate { get; set; }
    //    public decimal ReturnAmount { get; set; }
    //    public byte IsMonthlyPaid { get; set; }        
    //    public string Type { get; set; }
    //    public decimal Rate { get; set; }
    //    public byte IsFixed { get; set; }
    //    public int RuleID { get; set; }
    //}

    public class AdvancePaymentFilter : EmployeeFilterVM
    {
        public DateTime FromDate_Filter { get; set; }
        public DateTime ToDate_Filter { get; set; }
        public string Type_Filter { get; set; }
        public string OnlyDueOrBetweenDate_Filter { get; set; }
    }

    public class AdvancePaymentEmpListVM : EmployeeVM
    {
        public int AdvanceID { get; set; }
        public DateTime SanctionDate { get; set; }
        public string Type { get; set; }
        public decimal ApplyAmount { get; set; }
        public decimal SanctionAmount { get; set; }
        public decimal ReturnAmount { get; set; }
        public bool IsFixed { get; set; }
        public decimal Rate { get; set; }
        public string RuleName { get; set; }
        public decimal DueAmount { get; set; }
    }

    public class AdvancePaymentVM : Audit_Company
    {
        public int AdvanceID { get; set; }
        public int EmpId { get; set; }
        public decimal ApplyAmount { get; set; }
        public decimal SanctionAmount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime SanctionDate { get; set; }
        public decimal ReturnAmount { get; set; }
        public byte IsMonthlyPaid { get; set; }
        public string Type { get; set; }
        public decimal Rate { get; set; }
        public byte IsFixed { get; set; }
        public int RuleID { get; set; }
    }

    public class AdvanceSummaryReportFilter : EmployeeFilterVM
    {
        public DateTime UptoDate_Filter { get; set; }
        public DateTime? FromDate_Filter { get; set; }
        public DateTime? ToDate_Filter { get; set; }

        public string Type_Filter { get; set; }
        public string OnlyDueOrBetweenDate_Filter { get; set; }
    }

    public class AdvanceSummaryReportVM : EmployeeVM
    {
        public int AdvanceID { get; set; }
        //public decimal ApplyAmount { get; set; }
        public decimal SanctionAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime SanctionDate { get; set; }
        //public decimal ReturnAmount { get; set; }
        //public bool IsMonthlyPaid { get; set; }
        public string Type { get; set; }
        //public decimal Rate { get; set; }
        //public byte IsFixed { get; set; }
        //public int RuleID { get; set; }
    }
    public class AdvanceDetailsReportVM : EmployeeVM
    {
        public int AdvanceID { get; set; }
        //public decimal ApplyAmount { get; set; }
        public int DeptId { get; set; }
        public decimal SanctionAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int DesigId { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime SanctionDate { get; set; }

        //public decimal ReturnAmount { get; set; }
        //public bool IsMonthlyPaid { get; set; }
        public string Type { get; set; }
        //public decimal Rate { get; set; }
        //public byte IsFixed { get; set; }
        //public int RuleID { get; set; }
    }




    public class PromotionFilter : EmployeeFilterVM
    {

    }

    public class PromotionVM : Audit_Company
    {
        public int EmpId { get; set; }
        public int EmpHistoryId { get; set; }
        public int DesignationId { get; set; }
        public int PayScaleId { get; set; }
        public decimal Salary { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }

    }
    public class IncrementVM : Audit_Company
    {
        public int EmpId { get; set; }
        public int EmpHistoryId { get; set; }

        public decimal Amount { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Rate { get; set; }
        public bool IsFixed { get; set; }
        public int? CalcRuleID { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }

    }


    public class TransferVM : Audit_Company
    {
        public int EmpId { get; set; }
        public int EmpHistoryId { get; set; }
        public int SectId { get; set; }
        public int DeptId { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }

    }


    public class IncProTranReport_FilterVM : EmployeeFilterVM
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }


    public class IncrementReportVM : EmployeeVM
    {
        public decimal PrevSalary { get; set; }
        public decimal IncrAmount { get; set; }
        public decimal CurrSalary { get; set; }
        public float ServiceDays { get; set; }
        public bool IsFixed { get; set; }
        public float Rate { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }

    }

    public class PromotionReportVM : EmployeeVM
    {
        public string NewDes { get; set; }
        public string PrevDes { get; set; }
        public decimal PrevSalary { get; set; }
        public string PresentDes { get; set; }
        public string PresentSection { get; set; }
        public int PresentGrade { get; set; }
        public decimal PresentSalary { get; set; }
        public decimal JoiningSalary { get; set; }
        public decimal IncrAmount { get; set; }
        public decimal CurrSalary { get; set; }
        public float ServiceDays { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }

    }


    public class TransferReportVM : EmployeeVM
    {
        public string NewSec { get; set; }
        public string NewDept { get; set; }
        public string NewDes { get; set; }
        public string PrevSec { get; set; }
        public string PrevDept { get; set; }
        public string PrevDes { get; set; }
        public decimal PrevSalary { get; set; }
        public string PresentDes { get; set; }
        public string PresentDept { get; set; }
        public string PresentSection { get; set; }
        public int PresentGrade { get; set; }
        public decimal PresentSalary { get; set; }
        public decimal JoiningSalary { get; set; }
        public decimal CurrSalary { get; set; }
        public float ServiceDays { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }
    }







}
