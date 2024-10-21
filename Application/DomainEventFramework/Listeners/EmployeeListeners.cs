using Application.DomainEventFramework.Default;
using Application.Tasks.DomainEvents;
using Application.Tasks.DomainEvents.EventHandlers;
using Domains.ViewModels;
using Messaging.Framework.Common;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DomainEventFramework.Listeners
{
    public class EmployeeListeners : ConsumerBase
    {
        public EmployeeListeners() : base(RabbitMQProperties.ExchangeName, EventNames.InviteClient)
        {
        }

        public async override Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var handler = scope.ServiceProvider.GetService<EmployeeEventsHandlers>();
            var jObject = JObject.Parse(genericMessage.payload);
            var domain = jObject["DomainEventVM"].ToObject<DomainEventVM>();
            await handler.EmployeeCommandHandler(domain);
            return MessageAcknowledgement.Processed;
        }
    }
}
