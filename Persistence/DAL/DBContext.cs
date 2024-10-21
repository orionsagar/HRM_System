using Dapper;
using Domains.Models;
using Domains.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Persistence.Repository.Leave;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Domains.Models.Recruitment;

namespace Persistence.DAL
{

    public interface IApplicationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        //public void ClearChangeTracker();

        // ModuleMenu
        public DbSet<MainModule> MainModules { get; set; }
        public DbSet<SubModule> SubModule { get; set; }
        public DbSet<UserRolewiseModule> UserRolewiseModules { get; set; }
        public DbSet<UserRolewiseSubModule> UserRolewiseSubModules { get; set; }
        public DbSet<UserAccessList> UserAccessList { get; set; }
        public DbSet<UserAccessTools> UserAccessTools { get; set; }
        public DbSet<ReportAccess> ReportAccess { get; set; }
       // public DbSet<SystemLock> SystemLock { get; set; }


        // User

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        //HR 
        #region HR

        
        public DbSet<Country> Countrys { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Thana> Thanas { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<CalculationRule> CalculationRules { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<AnnualPay> AnnualPay { get; set; }
        public DbSet<PayScaleType> PayScaleTypes { get; set; }
        public DbSet<PayScale> PayScales { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<EmployeeCategory> EmployeeCategorys { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatus { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistorys { get; set; }
        public DbSet<EmployeeDetails> EmployeeDetailses { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public DbSet<EmployeeExperience> EmployeeExperiences { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<RuleDefined> RuleDefineds { get; set; }
        public DbSet<SalaryBreakDown> SalaryBreakDowns { get; set; }

        public DbSet<AdvancePayment> AdvancePayment { get; set; }
        public DbSet<AdvanceType> AdvanceType { get; set; }
        public DbSet<Advance> Advance { get; set; }
        #endregion

        // Schedule
        #region Schedule
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<JobCalender> JobCalenders { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<EmployeeHoliday> EmployeeHolidays { get; set; }
        public DbSet<EmployeeJobCalendar> EmployeeJobCalenders { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }
        #endregion

        // Leave
        #region Leave
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<LeavePayMethod> LeavePayMethod { get; set; }
        public DbSet<LeaveDays> LeaveDays { get; set; }
        public DbSet<FiscalYear> FiscalYear { get; set; }
        public DbSet<LeaveSetup> LeaveSetup { get; set; }
        public DbSet<LeaveTransaction> LeaveTransaction { get; set; }
        public DbSet<EarnLeave> EarnLeave { get; set; }
        public DbSet<ShortLeaveSetup> ShortLeaveSetup { get; set; }
        public DbSet<ShortLeaveAssign> ShortLeaveAssign { get; set; }
        #endregion

        //BankList
        #region BankList
        public DbSet<BankLists> BankLists { get; set; }

        #endregion

        // Attendance
        #region Attendance
        public DbSet<Attendance> Attendance { get; set; }
        //public DbSet<AttendanceBonus> AttendanceBonus { get; set; }
        public DbSet<AttendanceDownload> AttendanceDownload { get; set; }
        public DbSet<RTA_DOWNLOADED> RTA_DOWNLOADED { get; set; }
        public DbSet<HolidayPresentDays> HolidayPresentDays { get; set; }
        public DbSet<LatePresentDays> LatePresentDays { get; set; }
        public DbSet<PresentDays> PresentDays { get; set; }
        public DbSet<LunchAbsentDays> LunchAbsentDays { get; set; }
        #endregion

        // Rule
        public DbSet<CompanyRule> CompanyRules { get; set; }

        // Log
        public DbSet<EditDeleteInfo> EditDeleteInfo { get; set; }
        public DbSet<ProcessLog> ProcessLog { get; set; }


        // Salary
        #region 
        public DbSet<OTRule> OTRule { get; set; }
        public DbSet<EmployeeOTRule> EmployeeOTRule { get; set; }
        public DbSet<PFFeeDeposit> PFFeeDeposit { get; set; }

        public DbSet<EmployeeSalary> EmployeeSalary { get; set; }
        public DbSet<SalarySheet> SalarySheets { get; set; }
        public DbSet<SalarySheetGenerated> SalarySheetGenerated { get; set; }
        public DbSet<CustomeSalarySheet> CustomeSalarySheet { get; set; }
        public DbSet<SettingUpSalaryReport> SettingUpSalaryReport { get; set; }
        #endregion

        // BonusNAlloance
        #region
        public DbSet<Deduction> Deduction { get; set; }
        public DbSet<SnacksAllowance> SnacksAllowance { get; set; }
        public DbSet<OtherAllowance> OtherAllowance { get; set; }
        public DbSet<AttendanceBonus> AttendanceBonus { get; set; }
        public DbSet<PerformanceBonus> PerformanceBonus { get; set; }
        public DbSet<FestivalBonus> FestivalBonus { get; set; }
        public DbSet<FestivalBonusSetup> FestivalBonusSetup { get; set; }
        public DbSet<FestivalType> FestivalType { get; set; }
        #endregion

        #region Report
        public DbSet<ReportTypes> ReportTypes { get; set; }
        public DbSet<Reports> Reports { get; set; }
        #endregion


        #region Sayed
        public DbSet<MotherCompany> MotherCompanies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
        public DbSet<BillingMaster> BillingMaster { get; set; }
        public DbSet<BillingDetails> BillingDetails { get; set; }
        public DbSet<PaymentReceiptMaster> PaymentReceiptMaster { get; set; }
        public DbSet<PaymentReceiptDetails> PaymentReceiptDetails { get; set; }
        public DbSet<HrmOrganisation> Organisations { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentMoad> PaymentMoads { get; set; }
        public DbSet<PayDetails> PayDetails { get; set; }
        public DbSet<TaxMaster> TaxMasters { get; set; }
        public DbSet<PaymentGroup> PaymentGroup { get; set; }
        public DbSet<LeaveRule> LeaveRules { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<EmpNewDocuments> EmpNewDocuments { get; set; }
        #endregion

        public DbSet<Client> Clients { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<EmployeeDocuments> EmployeeDocuments { get; set; }
        public DbSet<RightToWorkCheck> RightToWorkChecks { get; set; }



        public DbSet<Level> Level { get; set; }
        public DbSet<OrgHierarchy> OrgHierarchy { get; set; }
        //public DbSet<AnnualPay> AnnualPay { get; set; }

    }

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
           
        }

        public DbSet<BankLists> BankLists { get; set; }

        #region Sayed
        public DbSet<MotherCompany> MotherCompanies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentMoad> PaymentMoads { get; set; }
        public DbSet<PayDetails> PayDetails { get; set; }
        public DbSet<TaxMaster> TaxMasters { get; set; }
        public DbSet<PaymentGroup> PaymentGroup { get; set; }
        public DbSet<LeaveRule> LeaveRules { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<EmpNewDocuments> EmpNewDocuments { get; set; }

        #endregion

        public DbSet<MainModule> MainModules { get; set; }
        public DbSet<SubModule> SubModule { get; set; }
        public DbSet<UserRolewiseModule> UserRolewiseModules { get; set; }
        public DbSet<UserRolewiseSubModule> UserRolewiseSubModules { get; set; }
        public DbSet<UserAccessList> UserAccessList { get; set; }
        public DbSet<UserAccessTools> UserAccessTools { get; set; }
        public DbSet<ReportAccess> ReportAccess { get; set; }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        //HR 
        public DbSet<Country> Countrys { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Thana> Thanas { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<CalculationRule> CalculationRules { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<PayScaleType> PayScaleTypes { get; set; }
        public DbSet<PayScale> PayScales { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<EmployeeCategory> EmployeeCategorys { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatus { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistorys { get; set; }
        public DbSet<EmployeeDetails> EmployeeDetailses { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public DbSet<EmployeeExperience> EmployeeExperiences { get; set; }
        public DbSet<RuleDefined> RuleDefineds { get; set; }
        public DbSet<SalaryBreakDown> SalaryBreakDowns { get; set; }

        // Schedule
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<JobCalender> JobCalenders { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<EmployeeHoliday> EmployeeHolidays { get; set; }
        public DbSet<EmployeeJobCalendar> EmployeeJobCalenders { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }

        // Leave
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<LeavePayMethod> LeavePayMethod { get; set; }
        public DbSet<LeaveDays> LeaveDays { get; set; }
        public DbSet<FiscalYear> FiscalYear { get; set; }
        public DbSet<LeaveSetup> LeaveSetup { get; set; }
        public DbSet<LeaveTransaction> LeaveTransaction { get; set; }
        public DbSet<EarnLeave> EarnLeave { get; set; }
        public DbSet<ShortLeaveSetup> ShortLeaveSetup { get; set; }
        public DbSet<ShortLeaveAssign> ShortLeaveAssign { get; set; }

        // Attendance
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<AttendanceDownload> AttendanceDownload { get; set; }
        public DbSet<RTA_DOWNLOADED> RTA_DOWNLOADED { get; set; }
        public DbSet<HolidayPresentDays> HolidayPresentDays { get; set; }
        public DbSet<LatePresentDays> LatePresentDays { get; set; }
        public DbSet<PresentDays> PresentDays { get; set; }
        public DbSet<LunchAbsentDays> LunchAbsentDays { get; set; }

        // Rule
        public DbSet<CompanyRule> CompanyRules { get; set; }

        // Log
        public DbSet<EditDeleteInfo> EditDeleteInfo { get; set; }
        public DbSet<ProcessLog> ProcessLog { get; set; }

        // Salary
        #region Salary
        public DbSet<AdvancePayment> AdvancePayment { get; set; }
        public DbSet<AdvanceType> AdvanceType { get; set; }
        public DbSet<Advance> Advance { get; set; }
        public DbSet<OTRule> OTRule { get; set; }
        public DbSet<EmployeeOTRule> EmployeeOTRule { get; set; }
        public DbSet<PFFeeDeposit> PFFeeDeposit { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalary { get; set; }
        public DbSet<SalarySheet> SalarySheets { get; set; }
        public DbSet<SalarySheetGenerated> SalarySheetGenerated { get; set; }
        public DbSet<CustomeSalarySheet> CustomeSalarySheet { get; set; }
        public DbSet<SettingUpSalaryReport> SettingUpSalaryReport { get; set; }
        #endregion

        // BonusNAlloance
        #region
        public DbSet<Deduction> Deduction { get; set; }
        public DbSet<SnacksAllowance> SnacksAllowance { get; set; }
        public DbSet<OtherAllowance> OtherAllowance { get; set; }
        public DbSet<AttendanceBonus> AttendanceBonus { get; set; }
        public DbSet<PerformanceBonus> PerformanceBonus { get; set; }
        public DbSet<FestivalBonus> FestivalBonus { get; set; }
        public DbSet<FestivalBonusSetup> FestivalBonusSetup { get; set; }
        public DbSet<FestivalType> FestivalType { get; set; }
        #endregion


        #region Report
        public DbSet<ReportTypes> ReportTypes { get; set; }
        public DbSet<Reports> Reports { get; set; }
        #endregion

        public DbSet<Client> Clients { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<HrmOrganisation> Organisations { get; set; }

        public DbSet<Level> Level { get; set; }
        public DbSet<OrgHierarchy> OrgHierarchy { get; set; }
        public DbSet<EmployeeDocuments> EmployeeDocuments { get; set; }


        public DbSet<BillingMaster> BillingMaster { get; set; }
        public DbSet<BillingDetails> BillingDetails { get; set; }
        public DbSet<PaymentReceiptMaster> PaymentReceiptMaster { get; set; }
        public DbSet<PaymentReceiptDetails> PaymentReceiptDetails { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<RightToWorkCheck> RightToWorkChecks { get; set; }
        public DbSet<AnnualPay> AnnualPay { get; set; }
        public IDbConnection Connection => Database.GetDbConnection();

        

        public void ClearChangeTracker()
        {
            this.ChangeTracker.Clear();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Employee>();
            //.Entries()
            //.Where(e => e.Entity is Audit && (e.State == EntityState.Added || e.State == EntityState.Modified));

            //string userId = DataEncryptionInPersistance.DecryptString(_httpContextAccessor.HttpContext.Request.Cookies["UserID"]);
            //string compId = DataEncryptionInPersistance.DecryptString(_httpContextAccessor.HttpContext.Request.Cookies["CompID"]);
            foreach (var entityEntry in entries)
            {
                entityEntry.Property(a => a.CardNo).IsModified = false;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // get all except is delete == true
            modelBuilder.Entity<Employee>().HasQueryFilter(a => !a.IsDelete);

            // unique Employee card no by company
            modelBuilder.Entity<Employee>().HasIndex(e => new { e.CardNo, e.CompId }).IsUnique(true);
            modelBuilder.Entity<MotherCompany>().HasIndex(e => e.MC_U_Key).IsUnique();
            modelBuilder.Entity<Company>().HasIndex(e => e.C_U_Key).IsUnique();
        }

    }


    public interface IApplicationReadDbConnection
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }

    public interface IApplicationWriteDbConnection : IApplicationReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<int> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }

    public class ApplicationReadDbConnection : IApplicationReadDbConnection, IDisposable
    {
        private readonly IDbConnection connection;
        public ApplicationReadDbConnection()
        {
            connection = new SqlConnection(AppConfig.DefaultConnection);
        }
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(sql, param, transaction);
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }

    public class ApplicationWriteDbConnection : IApplicationWriteDbConnection
    {
        private readonly IApplicationDbContext context;
        public ApplicationWriteDbConnection(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.ExecuteAsync(sql, param, transaction);
        }
        public async Task<int> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            var scalarD = await context.Connection.ExecuteScalarAsync(sql, param, transaction);
            return Convert.ToInt32(scalarD);
        }
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await context.Connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QuerySingleAsync<T>(sql, param, transaction);
        }
    }





}
