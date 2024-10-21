using Application.DomainEventFramework.Core;
using Application.SupportiveBL.Context;
using Application.SupportiveBL.UserInvitation;
using Application.Tasks.Commands.CClient;
using Application.Tasks.Common;
using Application.Tasks.DomainEvents;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Handlers.HClient
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDomainEventPublish publisher;
        private readonly IAuthorizationContext authContext;
        private readonly ITransactable transactable;

        public CreateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainEventPublish publisher, IAuthorizationContext authContext, ITransactable transactable)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.publisher = publisher;
            this.authContext = authContext;
            this.transactable = transactable;
        }

        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            // do all client operation here, including image save
            if (request.ClientViewModel.Logofile != null)
            {
                string logo = await ImageUtil.UploadAnImageTo("UpImages", request.ClientViewModel.Logofile, "clt_");
                request.ClientViewModel.Logo = logo;
            }
            request.ClientViewModel.ClientType = request.ClientViewModel.ClientType == "Others" ? request.ClientViewModel.ClientType1 : request.ClientViewModel.ClientType;
            var comp = _mapper.Map<Client>(request.ClientViewModel);
            try
            {
                // start transaction
                using var trans = await transactable.BeginNewTransationAsync();

                var result = await _unitOfWork.ClientRepo.Add(comp);

                // create a user for client super admin, if provided
                int userId = 0;
                if (!string.IsNullOrWhiteSpace(request.ClientViewModel.ClientSAEmail))
                {
                    var roleId = await _unitOfWork.UserRoles.GetRoleIDBy_U_RoleName(Constants.ClientSuperAdminUniqueRoleName);
                    UserInfo userInfo = new()
                    {
                        FirstName = request.ClientViewModel.Name,
                        UserName = request.ClientViewModel.ClientSAEmail,
                        Email = request.ClientViewModel.ClientSAEmail,
                        UserStatus = UserStatus.Pending.ToString(),
                        RoleID = roleId,
                        AddedBy = authContext.UserId,
                        AddedDate = DateTime.UtcNow,
                        CompId = 1, 
                        ClientId = comp.ClientID
                    };
                    await _unitOfWork.UserInfos.AddWithoutTransaction(userInfo);
                    userId = userInfo.UserID;
                }

                await trans.FinishTransactionAsync();
                // we won't create any invitation here since validating email address isn't a related function of this command.
                // we will emit the domain event here
                // publish whole result to the DomainEvent

                var clientCreatedEvent = new ClientCreatedDomainEvent(comp.ClientID, userId);
                publisher.Publish(EventNames.ClientCreated, clientCreatedEvent, authContext.UserId);
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
