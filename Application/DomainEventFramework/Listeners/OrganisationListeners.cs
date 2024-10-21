using Application.DomainEventFramework.Default;
using Application.Tasks.DomainEvents.EventHandlers;
using Application.Tasks.DomainEvents;
using Messaging.Framework.Common;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DomainEventFramework.Listeners
{
    public class OrganisationCreated : ConsumerBase
    {
        public OrganisationCreated()
            : base(RabbitMQProperties.ExchangeName, EventNames.OrgCreated)
        {
        }

        public override async Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var orgHandler = scope.ServiceProvider.GetService<OrganisationEventHandlers>();
            await orgHandler.OrganisationCreatedHandler(JsonConvert.DeserializeObject<OrganisationCreatedDomainEvent>(genericMessage.payload), genericMessage.user_id);

            return MessageAcknowledgement.Processed;
        }
    }
}
