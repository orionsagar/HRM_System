using Application.Tasks.Commands.CUserInfo;
using Application.Tasks.Handlers.HCompany;
using Application.Tasks.MappingProfiles;
using AutoMapper;
using Business;
using Business.Attendance;
using Business.BonusNAllowance;
using Business.HR;
using Business.Leave;
using Business.Report;
using Business.Salary;
using Business.Schedule;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DAL;
using Persistence.Repository;
using Persistence.Repository.Attendance;
using Persistence.Repository.BonusNAllowance;
using Persistence.Repository.EmployeeJobCalenders;
using Persistence.Repository.EmployeeShifts;
using Persistence.Repository.HR;
using Persistence.Repository.Leave;
using Persistence.Repository.ModuleMenu;
using Persistence.Repository.Report;
using Persistence.Repository.Salary;
using Persistence.Repository.Schedules;
using Persistence.Repository.UserLog;
using System;
using System.Reflection;
using Application.SupportiveBL.UserInvitation;
using Messaging.Configuration;
using UKHRM.Common;
using Persistence.Repository.Invitations;
using Application.SupportiveBL.Context;
using Application.DomainEventFramework.Listeners;
using Persistence.Repository.Clients;
using Domains.ViewModels;
using Application.SupportiveBL.Email;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Utility;
using Application.DomainEventFramework.Configuration.DependencyInjection;
using Persistence.Repository.Organisation;
using Persistence.Repository.Billing;
using Persistence.Repository.PaymentReceipt;
using Persistence.Repository.Organogram;
using Persistence.Repository.Package;
using Domains.Models;
using Persistence.Repository.Recruitment;
using Persistence.Repository.Tax;
using Persistence.Repository.PaymentTypes;
using Persistence.Repository.PaymentMoads;
using Persistence.Repository.BankList;
using Persistence.Repository.PaymentGroups;
using Persistence.Repository.RTWC;

namespace UKHRM.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
           IConfiguration config)
        {

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
            });

            // Domain event framework

            var messagingOptions = new MessagingOptions();
            config.GetSection(nameof(MessagingOptions)).Bind(messagingOptions);
            services.AddDomainEventFramework(Constants.InstanceName, config.GetSection(SMTPOptions.SMTPOptionPath), options => {
                options.HostUrl = messagingOptions.HostUrl;
                options.Port = messagingOptions.Port;
                options.Username = messagingOptions.Username;
                options.Password = messagingOptions.Password;
                options.VHost = messagingOptions.VHost;
            })
            .AddEventListener<ClientCreated>()
            .AddEventListener<InviteClientListeners>()
            .AddEventListener<OrganisationCreated>()
            .AddEventListener<Greetings>()
            .AddEventListener<CommonListeners>()
            .AddEventListener<EmployeeListeners>()
            .StartListening();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddTransient<HttpUtil>();

            // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IAuthorizationContext, AuthorizationContext>();


            #region Commands
            services.AddMediatR(typeof(CreateCompanyCommandHandler).Assembly);
            services.AddMediatR(typeof(CreateUserInfoCommand).Assembly);
            #endregion

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactable, Transactable>();

            services.AddScoped<ImageChecker>();
            services.AddScoped<CommonDropdown>();
            services.AddScoped<NumberGeneration>();
            services.AddScoped<ICustomRepository, CustomRepository>();
            services.AddScoped<IGlobalHelper, GlobalHelper>();

            #region User Invitation
            services.AddScoped<IUserInvitation, AccountInvitation>();
            services.AddScoped<IUserInvitation, PasswordInvitation>();

            // Add all implementations again to resolve them from a factory
            services.AddScoped<AccountInvitation>();
            services.AddScoped<PasswordInvitation>();

            // lastly, inject the Factory
            services.AddScoped<InviteFactory>();
            #endregion

            #region Main module Repo
            #region Sayed
            services.AddScoped<IMainModuleRepo, MainModuleRepository>();
            services.AddScoped<IUserAccessRepo, UserAccessRepository>();
            services.AddScoped<ISubModuleRepo, SubModuleRepository>();
            services.AddScoped<IModuleMenuRepo, ModuleMenuRepository>();
            #endregion
            #endregion

            #region User Repo
            #region Sayed
            services.AddScoped<IUserInfoRepo, UserInfoRepository>();
            services.AddScoped<IUserRoleRepo, UserRoleRepository>();
            #endregion
            #endregion

            #region HR Repo
            #region Sayed
            services.AddScoped<ISectionRepo, SectionRepo>();
            services.AddScoped<IEmployeeCategoryRepo, EmployeeCategoryRepo>();
            services.AddScoped<IDesignationRepo, DesignationRepo>();
            services.AddScoped<IPayScaleRepo, PayScaleRepo>();
            services.AddScoped<IRulesRepo, RulesRepository>();
            services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            services.AddScoped<IEmployeeTypeRepo, EmployeeTypeRepo>();
            #endregion

            #region Himu
            services.AddScoped<ILocationRepo, LocationRepo>();
            services.AddScoped<ICompanyRepo, CompanyRepo>();
            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IEmployeeStatusRepo, EmployeeStatusRepository>();
            services.AddScoped<IEmployeeOTRuleRepo, EmployeeOTRuleRepository>();
            services.AddScoped<IAdvanceSanctionRepo, AdvanceSanctionRepository>();
            services.AddScoped<IAdvancePaymentRepo, AdvancePaymentRepository>();
            #endregion
            #endregion

            #region Schedule Repo
            #region Sayed
            services.AddScoped<IScheduleRepo, ScheduleRepository>();
            services.AddScoped<IShiftRepo, ShiftRepository>();
            services.AddScoped<IBreakRepo, BreakRepository>();
            services.AddScoped<IHolidayRepo, HolidayRepository>();
            #endregion

            #region Himu
            services.AddScoped<IJobCalenderRepo, JobCalenderRepository>();
            services.AddScoped<IEmployeeJobCalendarRepo, EmployeeJobCalendarRepository>();
            services.AddScoped<IEmployeeShiftRepo, EmployeeShiftRepository>();
            #endregion
            #endregion

            #region Leave
            #region Sayed
            services.AddScoped<ILeaveTypeRepo, LeaveTypeRepository>();
            services.AddScoped<ILeavePayMethodRepo, LeavePayMethodRepository>();
            services.AddScoped<ILeaveSetupRepo, LeaveSetupRepository>();
            services.AddScoped<IEarnLeaveRepo, EarnLeaveRepository>();
            services.AddScoped<IFiscalYearRepo, FiscalYearRepository>();
            services.AddScoped<ILeaveTransactionRepo, LeaveTransactionRepository>();
            services.AddScoped<IShortLeaveSetup, ShortLeaveSetupRepository>();
            #endregion

            #region Himu
            services.AddScoped<IShortLeaveAssignRepo, ShortLeaveAssignRepository>();
            #endregion

            #endregion

            #region Attendance Repo
            #region Himu
            services.AddScoped<IAttendanceDataReader, ZKTAccessDBHelper>();
            services.AddScoped<IAttendanceTransferRepo, AttendanceTransferRepo>();
            services.AddScoped<IAttendanceRepo, AttendanceRepo>();
            #endregion
            #endregion


            #region Salary Repo
            #region Himu
            services.AddScoped<ISalarySheetRepo, SalarySheetRepo>();


            #endregion
            #endregion

            #region AllowanceNBonus Repo
            #region Himu
            services.AddScoped<ILunchSetupRepo, LunchSetupRepository>();
            services.AddScoped<IOtherAllowanceRepo, OtherAllowanceRepository>();
            services.AddScoped<IDeductionRepo, DeductionRepository>();
            services.AddScoped<IBonusRepo, BonusRepository>();

            #endregion
            #endregion

            #region Report Repo
            #region Sayed
            services.AddScoped<IManPowerReportRepo, ManPowerReportRepository>();
            services.AddScoped<ISalaryReportRepo, SalaryReportRepository>();
            services.AddScoped<IReportRepo, ReportRepository>();
            #endregion

            #region Himu
            services.AddScoped<IIncrPromTransReportRepo, IncrPromTransReportRepository>();            
            #endregion

            #endregion

            #region Log Repo
            #region Himu
            services.AddScoped<ILogRepo, LogRepo>();

            #endregion
            #endregion

            #region Dashboard
            #region Sayed
            services.AddScoped<IDashBoard, DashBoardReporitory>();
            #endregion
            #endregion


            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            #region HR BL
            #region Sayed
            services.AddScoped<IPayScaleBL, PayScaleBL>();
            #endregion

            #region Himu
            services.AddScoped<IDepartmentBL, DepartmentBL>();
            services.AddScoped<IEmployeeBL, EmployeeBL>();
            services.AddScoped<IEmployeeStatusBL, EmployeeStatusBL>();
            services.AddScoped<IEmployeeOTRuleBL, EmployeeOTRuleBL>();
            services.AddScoped<IAdvanceSanctionBL, AdvanceSanctionBL>();
            services.AddScoped<IAdvancePaymentBL, AdvancePaymentBL>();
            services.AddScoped<IPromotionBL, PromotionBL>();
            services.AddScoped<IIncrementBL, IncrementBL>();
            services.AddScoped<ITransferBL, TransferBL>();

            #endregion
            #endregion

            #region Schedule BL  
            #region Himu
            services.AddScoped<IEmployeeJobCalendarBL, EmployeeJobCalendarBL>();
            services.AddScoped<IEmployeeShiftBL, EmployeeShiftBL>();
            #endregion
            #endregion

            #region Leave BL  
            #region Sayed
            services.AddScoped<IEarnLeaveBL, EarnLeaveBL>();
            services.AddScoped<ILeaveTransactionBL, LeaveTransactionBL>();
            #endregion

            #region Himu
            services.AddScoped<IShortLeaveAssignBL, ShortLeaveAssignBL>();
            #endregion

            #endregion

            #region Attendance BL
            #region Himu
            services.AddScoped<IAttendanceBL, AttendanceBL>();
            services.AddScoped<IManualAttendanceBL, ManualAttendanceBL>();
            #endregion
            #endregion

            #region Salary BL
            #region Himu
            services.AddScoped<ISalaryBL, SalaryBL>();

            #endregion
            #endregion

            #region AllowanceNBonus BL
            #region Himu
            services.AddScoped<ILunchSetupBL, LunchSetupBL>();
            services.AddScoped<IOtherAllowanceBL, OtherAllowanceBL>();
            services.AddScoped<IDeductionBL, DeductionBL>();
            services.AddScoped<IAttendanceBonusBL, AttendanceBonusBL>();
            services.AddScoped<IFestivalBonusBL, FestivalBonusBL>();
            services.AddScoped<IPerformanceBonusBL, PerformanceBonusBL>();
            #endregion
            #endregion

            #region Report BL
            #region Himu
            services.AddScoped<IReportBL, ReportBL>();
            #endregion
            #endregion

            #region MAIN BL
            #region Sayed
            services.AddScoped<IDashboardBL, DashboardBL > ();
            services.AddScoped<IAuthBL, AuthBL> ();
            services.AddScoped<ICompanyBL, CompanyBL> ();
            #endregion
            #endregion

            services.AddScoped<IClientRepo, ClientRepository>();
            services.AddScoped<IReCaptchaClass, ReCaptchaClass>();
            services.AddScoped<IOrganisation, OrganisationRepository>();
            services.AddScoped<IBillingRepo, BillingRepository>();
            services.AddScoped<IPaymentReceiptRepo, PaymentReceiptRepository>();
            services.AddScoped<IPackage, PackageRepository>();
            services.AddScoped<IJobRepo, JobRepository>();
            services.AddScoped<IBankListRepo, BankListRepository> ();

            services.AddScoped<IInviteRepo, InviteRepo>();
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<IOrganogramRepository, OrganogramRepository>();
            services.AddScoped<IEmployeeDocumentRepo, EmployeeDocumentRepo>();
            services.AddScoped<IEmpNewDocumentRepo, EmpNewDocumentRepo>();
            services.AddScoped<IClientProfile, ClientProfileRepository>();
            services.AddScoped<ITasksRepo, TasksRepository>();
            services.AddScoped<ITaxMasterRepo, TaxMasterRepository>();
            services.AddScoped<IPaymentTypeRepo, PaymentTypeRepository>();
            services.AddScoped<IPaymentMoad, PaymentMoadRepository>();
            services.AddScoped<ILeaveRuleRepo, LeaveRuleRepository>();
            services.AddScoped<ILeaveAllocation, LeaveAllocationRepository>();
            services.AddScoped<IPaymentGroupRepo, PaymentGroupsRepository>();
            services.AddScoped<IRTWCRepository, RTWCRepository>();
            return services;
        }
    }
}
