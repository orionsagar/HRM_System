
using Application.DomainEventFramework.Core;
using Application.Tasks.Commands.CClient;
using Application.Tasks.DomainEvents;
using MediatR;
using Persistence.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HClient
{
    public class InviteClientCommandHandler : IRequestHandler<InviteClientCommand, string>
    {
        private readonly IDomainEventPublish publisher;
        

        public InviteClientCommandHandler(IDomainEventPublish publisher)
        {
            this.publisher = publisher;
        }

        public async Task<string> Handle(InviteClientCommand request, CancellationToken cancellationToken)
        {
            var clientCreatedEvent = new InviteClientEvent(request.InviteClient.Email,request.InviteClient.UserName);
            publisher.Publish(EventNames.InviteClient, clientCreatedEvent, request.InviteClient.AddedById.ToString());

            return "Invitation Send Successfully";
        }
    }
}
