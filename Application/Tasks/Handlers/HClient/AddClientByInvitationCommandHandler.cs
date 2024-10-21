using Application.DomainEventFramework.Core;
using Application.DomainEventFramework.Listeners;
using Application.Tasks.Commands.CClient;
using Application.Tasks.Common;
using Application.Tasks.DomainEvents;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Handlers.HClient
{
    public class AddClientByInvitationCommandHandler : IRequestHandler<AddClientByInvitationCommand, string>
    {
        private readonly IUnitOfWork _work;
        private readonly ITransactable transactable;
        private readonly IMapper _mapper;
        private readonly IDomainEventPublish publisher;

        public AddClientByInvitationCommandHandler(IUnitOfWork work, ITransactable transactable, IMapper mapper, IDomainEventPublish publisher)
        {
            _work = work;
            this.transactable = transactable;
            _mapper = mapper;
            this.publisher = publisher;
        }

        public async Task<string> Handle(AddClientByInvitationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var trans = await transactable.BeginNewTransationAsync();
                var client = _mapper.Map<Client>(request.Client);
                var result = await _work.ClientRepo.Add(client); // commend for test email send

               var greetin = new DomainEventVM
                {
                    ClientName = request.Client.Name,
                    MailFor = Enums.MailFor.Client.ToString(),
                    Email = request.Client.Email1
                };
                var greetings = new GreetingsDomainEvents(greetin);
                publisher.Publish(EventNames.Greetings, greetings, request.Client.UserId.ToString());


                var roleId = await _work.UserRoles.GetRoleIDBy_U_RoleName(Constants.ClientSuperAdminUniqueRoleName);
                UserInfo userInfo = new()
                {
                    FirstName = request.Client.Name,                    
                    UserName = request.Client.ClientSAEmail,
                    Email = request.Client.ClientSAEmail,
                    UserStatus = UserStatus.Pending.ToString(),
                    RoleID = roleId,
                    AddedBy = request.Client.UserId.ToString(),
                    AddedDate = DateTime.UtcNow,
                    CompId = 1,
                    ClientId = client.ClientID,
                    Password = request.Client.Password,
                    OrgId = 0
                };
                await _work.UserInfos.AddWithoutTransaction(userInfo);
                await trans.FinishTransactionAsync();
                return "Client Create Successfully";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
