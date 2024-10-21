using Application.DomainEventFramework.Default;
using Application.Tasks.DomainEvents;
using Application.Tasks.DomainEvents.EventHandlers;
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
    public class Greetings : ConsumerBase
    {
        public Greetings() : base(RabbitMQProperties.ExchangeName, EventNames.Greetings)
        {
        }

        public override async Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var clientHandler = scope.ServiceProvider.GetService<GreetingsDomainEventsHandlers>();
            await clientHandler.GreetingsHandler(JsonConvert.DeserializeObject<GreetingsDomainEvents>(genericMessage.payload));

            return MessageAcknowledgement.Processed;
        }
    }
}
