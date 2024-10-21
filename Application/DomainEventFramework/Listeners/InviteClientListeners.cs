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
    public class InviteClientListeners : ConsumerBase
    {
        public InviteClientListeners()
            : base(RabbitMQProperties.ExchangeName, EventNames.InviteClient)
        {
        }
        public override async Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var clientHandler = scope.ServiceProvider.GetService<InviteClientEventHandlers>();
            if(clientHandler != null)
            {
                await clientHandler.InviteClientHandler(JsonConvert.DeserializeObject<InviteClientEvent>(genericMessage.payload), genericMessage.user_id);                
            }
            return MessageAcknowledgement.Processed;
        }
    }
}
