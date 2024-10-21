using Application.DomainEventFramework.Core;
using Application.SupportiveBL.Context;
using Application.Tasks.Commands.COrganisation;
using Application.Tasks.DomainEvents;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;
using Application.Tasks.Common;

namespace Application.Tasks.Handlers.HOrganisation
{    
    public class OrganisationCommandHandler : IRequestHandler<OrganisationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDomainEventPublish publisher;
        private readonly IAuthorizationContext authContext;
        private readonly ITransactable transactable;
        public OrganisationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventPublish publisher, IAuthorizationContext authContext, ITransactable transactable)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.publisher = publisher;
            this.authContext = authContext;
            this.transactable = transactable;
        }
        
        public async Task<int> Handle(OrganisationCommand request, CancellationToken cancellationToken)
        {
            // do all client operation here, including image save
            if (request.OrganisationVM.Logofile != null)
            {
                string logo = await ImageUtil.UploadAnImageTo("UpImages", request.OrganisationVM.Logofile, "org_");
                request.OrganisationVM.LogoPath = logo;
            }

            if (request.OrganisationVM.KC_NId != null)
            {
                string logo = await ImageUtil.UploadAnImageTo("UpImages", request.OrganisationVM.KC_NId, "org_");
                request.OrganisationVM.KC_NIdPath = logo;
            }
            if (request.OrganisationVM.AP_NId != null)
            {
                string logo = await ImageUtil.UploadAnImageTo("UpImages", request.OrganisationVM.AP_NId, "org_");
                request.OrganisationVM.AP_NIdPath = logo;
            }
            request.OrganisationVM.SectorName = request.OrganisationVM.SectorName != "Other service activities" ? request.OrganisationVM.SectorName : request.OrganisationVM.SectorName1;
            request.OrganisationVM.OrgType = request.OrganisationVM.OrgType != "others-type" ? request.OrganisationVM.OrgType : request.OrganisationVM.OrgType1;
            var organisation = _mapper.Map<HrmOrganisation>(request.OrganisationVM);
            var addate = organisation.OrgId > 0 ? organisation.AddedDate : DateTime.Now;
            organisation.AddedDate = addate;
            try
            {
                // start transaction
                using var trans = await transactable.BeginNewTransationAsync();
                var result = 0;
                // ============  Organisation Entry And Update =============== //
                if (organisation.OrgId> 0)
                {
                    organisation.UpdatedBy = request.OrganisationVM.AddedBy;
                    await _unitOfWork.OrgRepo.Update(organisation);
                    result = organisation.OrgId;
                }
                else
                {
                    organisation.Status = Enums.Status.Active.ToString();
                    result = await _unitOfWork.OrgRepo.Add(organisation);
                }

                // =============== PayScale Ente ============== //
                var sbd = new SalaryBreakDown
                {
                    CompId = 1,
                    ConveyanceRate = 10,
                    MedicalRate = 10,
                    FoodingRate = 10,
                    SalaryRate = 50,
                    HouseRentRate = 20,
                    RuleID = 21,
                    OrgId = organisation.OrgId,
                };
                var payscale = new PayScale
                {
                    PayScaleTypeId = 6,
                    GradeOrScale = 1,
                    InitialPay = 100,
                    ByGross = true,
                    AddedBy = request.OrganisationVM.AddedBy,
                    AddedDate = DateTime.UtcNow,
                    CompId = 1,
                    OrgId = organisation.OrgId,
                    SalaryBreakDown = sbd

                };
                await _unitOfWork.PayScales.AddPayScaleWithSalaryBreakDown(payscale);

                // ===================== User Entry ====================== //
                // create a user for client super admin, if provided
                int userId = 0;
                bool isUserSaved = false;
                if (!string.IsNullOrWhiteSpace(request.OrganisationVM.SA_Email))
                {
                    var roleId = await _unitOfWork.UserRoles.GetRoleIDBy_U_RoleName(Constants.OrgSuperAdminUniqueRoleName);

                    var user = await _unitOfWork.UserInfos.GetByOrgId_And_RoleId(organisation.OrgId,roleId);
                    if(user == null)
                    {
                        UserInfo userInfo = new()
                        {
                            FirstName = request.OrganisationVM.SA_FirstName,
                            LastName = request.OrganisationVM.SA_LastName,
                            UserName = request.OrganisationVM.SA_Email,
                            Email = request.OrganisationVM.SA_Email,
                            UserStatus = UserStatus.Pending.ToString(),
                            RoleID = roleId,
                            AddedBy = organisation.AddedBy,
                            AddedDate = DateTime.UtcNow,
                            CompId = 1,
                            OrgId = organisation.OrgId,
                            ClientId = request.OrganisationVM.ClientId,
                        };
                        await _unitOfWork.UserInfos.AddWithoutTransaction(userInfo);
                        userId = userInfo.UserID;
                        isUserSaved = true;
                    }
                    else
                    {
                        if(user.UserStatus == UserStatus.Pending.ToString())
                        {
                            user.FirstName = request.OrganisationVM.SA_FirstName;
                            user.LastName = request.OrganisationVM.SA_LastName;
                            user.Email = request.OrganisationVM.SA_Email;
                            user.UserName = request.OrganisationVM.SA_Email;
                            user.ClientId = request.OrganisationVM.ClientId;
                            await _unitOfWork.UserInfos.UpdateWithoutTrans(user);
                            userId = user.UserID;
                            isUserSaved = true;
                        }
                    }                                                      
                }

                await trans.FinishTransactionAsync();
                // we won't create any invitation here since validating email address isn't a related function of this command.
                // we will emit the domain event here
                // publish whole result to the DomainEvent
                if(isUserSaved)
                {
                    var orgCreatedEvent = new OrganisationCreatedDomainEvent(organisation.OrgId, userId);
                    publisher.Publish(EventNames.OrgCreated, orgCreatedEvent, authContext.UserId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                // need to set a logger here 
                return 0;
            }
        }
    }
}
