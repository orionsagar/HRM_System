using Application.DomainEventFramework.Core;
using Application.Tasks.Commands.CUserInfo;
using Application.Tasks.DomainEvents;
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

namespace Application.Tasks.Handlers.HUserInfo
{
    public class PasswordResetCommandHandler : IRequestHandler<PasswordResetCommand, string>
    {
        private readonly IUnitOfWork _work;
        private readonly IDomainEventPublish _publisher;

        public PasswordResetCommandHandler(IUnitOfWork work, IDomainEventPublish publisher)
        {
            _work = work;
            _publisher = publisher;
        }

        public async Task<string> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
        {
            request.DomainEvents.MailFor = Enums.MailFor.PasswordReset.ToString();
            request.DomainEvents.UserName = request.DomainEvents.Email;
            var clientCreatedEvent = new CommonEvents(request.DomainEvents);
            _publisher.Publish(EventNames.Alert, clientCreatedEvent, request.DomainEvents.UserId.ToString());
            return "Invitation Send Successfully";
        }
    }
}
