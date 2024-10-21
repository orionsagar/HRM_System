using DomainEventFramework.Default;
using DomainEventFramework.DomainEvent;
using Messaging.Framework.Common;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DomainEventFramework.Listeners
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
            // var 
                throw new NotImplementedException();
        }
    }
}
