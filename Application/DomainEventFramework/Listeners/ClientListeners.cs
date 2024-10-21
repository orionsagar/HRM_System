using Application.DomainEventFramework.Default;
using Application.Tasks.DomainEvents;
using Application.Tasks.DomainEvents.EventHandlers;
using Messaging.Framework.Common;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Application.DomainEventFramework.Listeners
{
    public class ClientCreated : ConsumerBase
    {
        public ClientCreated() 
            : base(RabbitMQProperties.ExchangeName, EventNames.ClientCreated)
        {
        }

        public override async Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var clientHandler = scope.ServiceProvider.GetService<ClientEventHandlers>();
            await clientHandler.ClientCreatedHandler(JsonConvert.DeserializeObject<ClientCreatedDomainEvent>(genericMessage.payload), genericMessage.user_id);

            return MessageAcknowledgement.Processed;
        }
    }
}
