using Domains.Models;
using Persistence.Repository;
using Persistence.Repository.Attendance;
using Persistence.Repository.BankList;
using Persistence.Repository.Billing;
using Persistence.Repository.BonusNAllowance;
using Persistence.Repository.Clients;
using Persistence.Repository.EmployeeJobCalenders;
using Persistence.Repository.EmployeeShifts;
using Persistence.Repository.HR;
using Persistence.Repository.Invitations;
using Persistence.Repository.Leave;
using Persistence.Repository.ModuleMenu;
using Persistence.Repository.Organisation;
using Persistence.Repository.Organogram;
using Persistence.Repository.Package;
using Persistence.Repository.PaymentMoads;
using Persistence.Repository.PaymentReceipt;
using Persistence.Repository.PaymentTypes;
using Persistence.Repository.Recruitment;
using Persistence.Repository.Report;
using Persistence.Repository.Salary;
using Persistence.Repository.Schedules;
using Persistence.Repository.Tax;
using Persistence.Repository.UserLog;
using Persistence.Repository.PaymentGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Repository.RTWC;

namespace Persistence.DAL
{
    public interface IUnitOfWork
    {
        //Module Menu
        #region Module Menu
        #region Sayed
        IMainModuleRepo MainModules { get; }
        ISubModuleRepo SubModule { get; }
        IModuleMenuRepo ModuleMenu { get; }
        IUserAccessRepo UserAccess { get; }
        #endregion
        #endregion
        //User
        #region User
        #region Sayed
        IUserInfoRepo UserInfos { get; }
        IUserRoleRepo UserRoles { get; }
        #endregion
        #endregion
        // HR
        #region HR
        #region Sayed        
        IDepartmentRepo Departments { get; }
        ISectionRepo Sections { get; }
        IDesignationRepo Designations { get; }
        IAnnualPayRepo AnnualPay { get; }
        IPayScaleRepo PayScales { get; }
        IEmployeeCategoryRepo EmployeeCategorys { get; }
        IEmployeeTypeRepo EmployeeTypes { get; }
        IRulesRepo Rules { get; }
        ITaxMasterRepo Taxs { get; }
        IPaymentTypeRepo PaymentType { get; }
        IPaymentMoad PaymentMoad { get; }
        
        #endregion

        #region Himu
        IEmployeeStatusRepo EmployeeStatus { get; }
        IEmployeeRepo Employees { get; }
        ILocationRepo Locations { get; }
        ICompanyRepo Companys { get; }
        IEmployeeOTRuleRepo EmployeeOTRules { get; }
        IAdvanceSanctionRepo AdvanceSanction { get; }
        IAdvancePaymentRepo AdvancePayment { get; }

        #endregion
        #endregion
        // Schedule
        #region Schedule
        #region Sayed
        IScheduleRepo Schedules { get; }
        IShiftRepo Shifts { get; }
        IBreakRepo Breaks { get; }
        IHolidayRepo Holidays { get; }
        #endregion

        #region Himu
        IJobCalenderRepo JobCalenders { get; }
        IEmployeeJobCalendarRepo EmployeeJobCalendars { get; }
        IEmployeeShiftRepo EmployeeShifts { get; }
        #endregion

        #endregion

        //BankList
        IBankListRepo BankLists { get; }

        //PaymentGroup
        IPaymentGroupRepo PaymentGroup { get; }
        // Leave
        #region Leave
        #region Sayed
        ILeaveTypeRepo LeaveTypes { get; }
        ILeavePayMethodRepo LeavePayMethods { get; }
        ILeaveSetupRepo LeaveSetups { get; }
        IEarnLeaveRepo EarnLeave { get; }
        ILeaveTransactionRepo LeaveTransaction { get; }
        IFiscalYearRepo FiscalYear { get; }
        IShortLeaveSetup ShortLeaveSetup { get; }
        ILeaveRuleRepo LeaveRule { get; }
        ILeaveAllocation LeaveAllocation { get; }
        #endregion

        #region Himu
        IShortLeaveAssignRepo ShortLeaveAssign { get; }
        #endregion
        #endregion

        // Attendance
        #region Attendance
        #region Himu
        IAttendanceRepo ManualAttendance { get; }
        #endregion
        #endregion

        // Salary
        #region Salary
        #region Himu
        ISalarySheetRepo SalarySheet { get; }

        #endregion
        #endregion

        // AllowanceNBonus
        #region AllowanceNBonus
        #region Himu
        ILunchSetupRepo LunchSetup { get; }
        IOtherAllowanceRepo OtherAllowance { get; }
        IDeductionRepo Deduction { get; }
        IBonusRepo Bonus { get; }
        #endregion
        #endregion

        // Report
        #region Report
        #region Sayed
        IManPowerReportRepo ManPowerReport { get; }
        ISalaryReportRepo SalaryReport { get; }
        IReportRepo Report { get; }
        #endregion

        #region Himu
        IIncrPromTransReportRepo IncrPromTransReportRepo { get; }
        #endregion


        #endregion

        // Log
        #region Log
        #region Himu
        ILogRepo LogRepo { get; }

        #endregion
        #endregion

        #region Dashboard
        #region Sayed
        IDashBoard DashBoard { get; }
        #endregion
        #endregion


        // Client

        IClientRepo ClientRepo { get; }
        IInviteRepo InvitationRepo { get; }
        IOrganisation OrgRepo { get; }
        IEmployeeDocumentRepo _EmployeeDocument { get; }
        IEmpNewDocumentRepo _EmpNewDocument { get; set; }


        ILevelRepository _levelRepository { get; }
        IOrganogramRepository _organogramRepository { get; }
        IBillingRepo BillingRepo { get; }
        IPaymentReceiptRepo PaymentReceiptRepo { get; }
        IPackage Package { get; }
        IJobRepo JobRepo { get; }
        ITasksRepo TasksRepo { get; }
        IRTWCRepository _RTWCRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {

        //Main Module
        #region Main Module
        #region Sayed
        public IMainModuleRepo MainModules { get; }
        public IUserAccessRepo UserAccess { get; }
        public ISubModuleRepo SubModule { get; }
        public IModuleMenuRepo ModuleMenu { get; }
        #endregion
        #endregion
        //User
        #region User
        #region Sayed
        public IUserInfoRepo UserInfos { get; }
        public IUserRoleRepo UserRoles { get; }
        #endregion
        #endregion
        // HR
        #region HR
        #region Sayed
        public IDepartmentRepo Departments { get; }
        public ISectionRepo Sections { get; }
        public IDesignationRepo Designations { get; }
        public IPayScaleRepo PayScales { get; }
        public IEmployeeCategoryRepo EmployeeCategorys { get; }
        public IEmployeeTypeRepo EmployeeTypes { get; }
        public IRulesRepo Rules { get; }
        public ITaxMasterRepo Taxs { get; }
        public IPaymentTypeRepo PaymentType { get; }
        public IPaymentMoad PaymentMoad { get; }
        #endregion

        #region Himu
        public IEmployeeStatusRepo EmployeeStatus { get; }
        public IEmployeeRepo Employees { get; }
        public ILocationRepo Locations { get; }
        public ICompanyRepo Companys { get; }
        public IEmployeeOTRuleRepo EmployeeOTRules { get; }
        public IAdvanceSanctionRepo AdvanceSanction { get; }
        public IAdvancePaymentRepo AdvancePayment { get; }
        #endregion
        #endregion

        // Schedule
        #region Schedule
        #region Sayed
        public IScheduleRepo Schedules { get; }
        public IShiftRepo Shifts { get; }
        public IBreakRepo Breaks { get; }
        public IHolidayRepo Holidays { get; }
        public IJobCalenderRepo JobCalenders { get; }
        #endregion

        #region Himu
        public IEmployeeJobCalendarRepo EmployeeJobCalendars { get; }
        public IEmployeeShiftRepo EmployeeShifts { get; }
        #endregion
        #endregion

        // Leave
        #region Leave
        #region Sayed
        public ILeaveTypeRepo LeaveTypes { get; }
        public ILeavePayMethodRepo LeavePayMethods { get; }
        public ILeaveSetupRepo LeaveSetups { get; }
        public IEarnLeaveRepo EarnLeave { get; }
        public ILeaveTransactionRepo LeaveTransaction { get; }
        public IFiscalYearRepo FiscalYear { get; }
        public IShortLeaveSetup ShortLeaveSetup { get; }
        public ILeaveRuleRepo LeaveRule { get; }
        public ILeaveAllocation LeaveAllocation { get; }
        #endregion

        #region Himu
        public IShortLeaveAssignRepo ShortLeaveAssign { get; }
        #endregion
        #endregion


        // Attendance
        #region Attendance

        #region Himu
        public IAttendanceRepo ManualAttendance { get; }
        #endregion
        #endregion

        // Salary
        #region Salary

        #region Himu
        public ISalarySheetRepo SalarySheet { get; }
        #endregion

        #endregion

        // AlloanceNBonus
        #region AlloanceNBonus

        #region Himu
        public ILunchSetupRepo LunchSetup { get; }
        public IOtherAllowanceRepo OtherAllowance { get; }
        public IDeductionRepo Deduction { get; }
        public IBonusRepo Bonus { get; }
        #endregion
        #endregion

        // Report
        #region Report
        #region Sayed
        public IManPowerReportRepo ManPowerReport { get; }
        public ISalaryReportRepo SalaryReport { get; }
        public IReportRepo Report { get; }
        #endregion

        #region Himu
        public IIncrPromTransReportRepo IncrPromTransReportRepo { get; }

        #endregion


        #endregion

        // Log
        #region Report
        #region Himu
        public ILogRepo LogRepo { get; }

        #endregion
        #endregion



        #region Dashboard
        #region Sayed
        public IDashBoard DashBoard { get; }
        #endregion
        #endregion

        //
        public IPaymentGroupRepo PaymentGroup { get; }
        public IBankListRepo BankLists { get; set; }

        //public IClientRepo ClientRepo { get; }

        public IClientRepo ClientRepo { get; }
        public IInviteRepo InvitationRepo { get; set; }
        public IOrganisation OrgRepo { get; set; }
        public IBillingRepo BillingRepo { get; }
        public IPaymentReceiptRepo PaymentReceiptRepo { get; }
        public IPackage Package { get; }
        public IJobRepo JobRepo { get; }
        public ITasksRepo TasksRepo { get; }
        public ILevelRepository _levelRepository { get; set; }
        //public IOrganogramRepository _oganogramRepository { get; set; }

        public IOrganogramRepository _organogramRepository { get; set; }
        public IEmployeeDocumentRepo _EmployeeDocument { get; set; }
        public IEmpNewDocumentRepo _EmpNewDocument { get; set; }
        public IRTWCRepository _RTWCRepository { get; set; }

        public IAnnualPayRepo AnnualPay { get; set; }
        public UnitOfWork(IDepartmentRepo department, IEmployeeRepo employee, ICompanyRepo company, IUserInfoRepo userInfos, 
            IMainModuleRepo mainModules, IUserAccessRepo userAccess, IUserRoleRepo userRoles = null, 
            ISectionRepo sections = null, IDesignationRepo designations = null, IEmployeeCategoryRepo employeeCategorys = null, 
            IEmployeeTypeRepo employeeTypes = null, IPayScaleRepo payScales = null, ILocationRepo locations = null, 
            ISubModuleRepo subModule = null, IModuleMenuRepo moduleMenu = null, IRulesRepo rules = null, IEmployeeStatusRepo employeeStatus = null, 
            IScheduleRepo schedules = null, IEmployeeJobCalendarRepo employeeJobCalendars = null, IJobCalenderRepo jobCalenders = null, 
            IShiftRepo shifts = null, IBreakRepo breaks = null, IHolidayRepo holidays = null, IEmployeeShiftRepo employeeShifts = null, 
            ILeaveTypeRepo leaveTypes = null, ILeavePayMethodRepo leavePayMethods = null, ILeaveSetupRepo leaveSetupRepos = null, IFiscalYearRepo fiscalYear = null, 
            IEarnLeaveRepo earnLeave = null, ILeaveTransactionRepo leaveTransaction = null, IAttendanceRepo manualAttendance = null, ISalarySheetRepo salarySheet = null, 
            IEmployeeOTRuleRepo employeeOTRules = null, IManPowerReportRepo manPowerReport = null, IAdvanceSanctionRepo advanceSanction = null, IAdvancePaymentRepo advancePayment = null, 
            IDeductionRepo deduction = null, ILunchSetupRepo lunchSetup = null, IOtherAllowanceRepo otherAllowance = null, IBonusRepo bonus = null, ISalaryReportRepo salaryReport = null, 
            ILogRepo logRepo = null, IShortLeaveSetup shortLeaveSetup = null, IShortLeaveAssignRepo shortLeaveAssign = null, IIncrPromTransReportRepo incrPromTransReportRepo = null, 
            IDashBoard dashBoard = null, IReportRepo report = null, IClientRepo clientRepo=null, IInviteRepo inviteRepo = null, IOrganisation orgRepo = null,
            ILevelRepository levelRepository=null, IOrganogramRepository organogramRepository=null, IBillingRepo billingRepo = null, IPaymentReceiptRepo paymentReceiptRepo = null,
            IEmployeeDocumentRepo employeeDocumentRepo = null, IPackage package = null, IJobRepo jobRepo = null, ITasksRepo tasksRepo = null, ITaxMasterRepo taxMasterRepo = null, 
            IEmpNewDocumentRepo empnewDocumentRepo = null, IPaymentTypeRepo paymentType = null, IPaymentMoad paymentMoad = null,IBankListRepo bankLists=null, ILeaveRuleRepo leaveRule = null, 
            ILeaveAllocation leaveAllocation = null, IPaymentGroupRepo paymentGroup = null, IRTWCRepository rTWCRepository = null, IAnnualPayRepo annualPay = null)
        {
            UserInfos = userInfos;
            MainModules = mainModules;
            UserAccess = userAccess;
            UserRoles = userRoles;
            BankLists = bankLists;
            Companys = company;
            Departments = department;
            Sections = sections;
            Designations = designations;
            EmployeeCategorys = employeeCategorys;
            EmployeeTypes = employeeTypes;
            Employees = employee;
            PayScales = payScales;
            Locations = locations;
            SubModule = subModule;
            ModuleMenu = moduleMenu;
            Rules = rules;
            EmployeeStatus = employeeStatus;
            Schedules = schedules;
            JobCalenders = jobCalenders;
            EmployeeJobCalendars = employeeJobCalendars;
            Shifts = shifts;
            Breaks = breaks;
            Holidays = holidays;
            EmployeeShifts = employeeShifts;
            LeaveTypes = leaveTypes;
            LeavePayMethods = leavePayMethods;
            LeaveSetups = leaveSetupRepos;
            FiscalYear = fiscalYear;
            EarnLeave = earnLeave;
            LeaveTransaction = leaveTransaction;
            ManualAttendance = manualAttendance;
            SalarySheet = salarySheet;
            EmployeeOTRules = employeeOTRules;
            ManPowerReport = manPowerReport;
            AdvanceSanction = advanceSanction;
            AdvancePayment = advancePayment;
            Deduction = deduction;
            LunchSetup = lunchSetup;
            OtherAllowance = otherAllowance;
            Bonus = bonus;
            SalaryReport = salaryReport;
            LogRepo = logRepo;
            ShortLeaveSetup = shortLeaveSetup;
            ShortLeaveAssign = shortLeaveAssign;
            IncrPromTransReportRepo = incrPromTransReportRepo;
            DashBoard = dashBoard;
            Report = report;
            ClientRepo = clientRepo;
            InvitationRepo = inviteRepo;
            OrgRepo = orgRepo;
            BillingRepo = billingRepo;
            PaymentReceiptRepo = paymentReceiptRepo;
            _levelRepository = levelRepository;
            _organogramRepository = organogramRepository;
            _EmployeeDocument = employeeDocumentRepo;
            _EmpNewDocument = empnewDocumentRepo;
            Package = package;
            JobRepo = jobRepo;
            TasksRepo = tasksRepo;
            Taxs = taxMasterRepo;
            PaymentType = paymentType;
            PaymentMoad = paymentMoad;
            LeaveRule = leaveRule;
            LeaveAllocation = leaveAllocation;
            PaymentGroup = paymentGroup;
            _RTWCRepository = rTWCRepository;
            AnnualPay = annualPay;
        }
    }
}
