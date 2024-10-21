using Application.SupportiveBL.UserInvitation;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using System.Globalization;
using System;
using static Domains.Models.Recruitment;
using static Domains.ViewModels.RecruitmentVM;

namespace Application.Tasks.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            #region Sagar
            CreateMap<Company, CompanyViewModel>()
                .ForMember(dest => dest.Logofile, src => src.Ignore())
                .ReverseMap();
            CreateMap<UserInfo, UserInfoViewModel>()
                   .ForMember(dest => dest.wrong_login_attempt, src => src.Ignore())
                   .ForMember(dest => dest.today_login_attempt, src => src.Ignore())
                   .ReverseMap();
            CreateMap<UserRole, UserRoleViewModel>().ReverseMap();

            CreateMap<Client, ClientViewModel>()
                .ForMember(dest => dest.ROWNUM, src => src.Ignore())
                .ForMember(dest => dest.TOTALCOUNT, src => src.Ignore())
                .ReverseMap();


            CreateMap<Level, LevelViewModels>().ReverseMap();

            //CreateMap<UserAccesstools, UserAccessViewModel>().ReverseMap();
            //CreateMap<UserRolewiseModule, UserRoleModuleIViewModel>().ReverseMap();
            //CreateMap<Supplier, SupplierViewModel>().ReverseMap();
            //CreateMap<CUnit, CUnitViewModel>().ReverseMap();
            //CreateMap<Brand, BrandViewModel>()
            //   .ForMember(dest => dest.ImageFile, src => src.Ignore())
            //   .ReverseMap();
            //CreateMap<DefaultProductSize, DProductSizeViewModel>().ReverseMap();
            //CreateMap<Products, ProductViewModel>()
            //    .ForMember(dest => dest.CatID, src => src.Ignore())
            //    .ForMember(dest => dest.ProductSizeID, src => src.Ignore())
            //    .ReverseMap();
            //CreateMap<Category, CategoryViewModel>().ReverseMap();


            #endregion

            #region Sayed Hossain
            CreateMap<MainModule, MainModuleViewModel>().ReverseMap();
                CreateMap<SubModule, SubModuleVM>().ReverseMap();
                CreateMap<PayScale, PayScaleVM>().ReverseMap();
                CreateMap<HrmOrganisation, OrganisationVM>().ReverseMap();
                CreateMap<BillingMaster, BillingMasterVM>().ReverseMap();
                CreateMap<BillingDetails, BillingDetailsVM>().ReverseMap();
                CreateMap<PaymentReceiptMaster, PaymentReceiptMasterVM>().ReverseMap();
                CreateMap<PaymentReceiptDetails, PaymentReceiptDetailsVM>().ReverseMap();
                CreateMap<Job, JobVM>().ReverseMap();
                CreateMap<LeaveRule, LeaveRuleVM>().ReverseMap();
            #endregion

            #region Invitation
            CreateMap<InviteModel, Invitation>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Input)).ReverseMap();
            CreateMap<EmployeeDocuments, EmployeeDocumentsVM>()
                .ForMember(dest => dest.UploadDocumentFile, src => src.Ignore())
                .ForMember(dest => dest.VisaUploadDocumentFile, src => src.Ignore())
                .ForMember(dest => dest.EussUploadDocumentFile, src => src.Ignore())
                .ForMember(dest => dest.DBSUploadDocumentFile, src => src.Ignore())
                .ForMember(dest => dest.IdUploadDocumentFile, src => src.Ignore())
                .ForMember(dest => dest.OtherUploadDocumentFile, src => src.Ignore())
                .ReverseMap();
            #endregion


            CreateMap<EmpNewDocuments, EmpDocumentsVM>()
                .ForMember(dest => dest.Files, src => src.Ignore())
                .ForMember(dest => dest.NinetyDaysNotify, src => src.Ignore())
                .ForMember(dest => dest.SixtyDaysNotify, src => src.Ignore())
                .ForMember(dest => dest.ThirtyDaysNotify, src => src.Ignore())
                .ForMember(dest => dest.totalcount, src => src.Ignore())
                .ForMember(dest => dest.rownum, src => src.Ignore())
                .ForMember(dest => dest.AllFiles, src => src.Ignore())
                .ReverseMap();


            CreateMap<LeaveRule, LeaveRuleVM>()
                .ForMember(dest => dest.EffectiveFrom, opt => opt.MapFrom(src => src.EffectiveFrom.ToString("dd-MM-yyyy")))
                .ForMember(dest => dest.EffectiveTo, opt => opt.MapFrom(src => src.EffectiveTo.ToString("dd-MM-yyyy")))
                .ReverseMap()
                .ForMember(dest => dest.EffectiveFrom, opt => opt.MapFrom(src => DateTime.ParseExact(src.EffectiveFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.EffectiveTo, opt => opt.MapFrom(src => DateTime.ParseExact(src.EffectiveTo, "dd-MM-yyyy", CultureInfo.InvariantCulture)));

            CreateMap<LeaveAllocation, LeaveAllocationVM>()
                .ReverseMap();

        }
    }
}
