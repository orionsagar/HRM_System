using Application.DomainEventFramework.Default;
using Application.Tasks.DomainEvents;
using Application.Tasks.DomainEvents.EventHandlers;
using Domains.ViewModels;
using Messaging.Framework.Common;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DomainEventFramework.Listeners
{
    public class CommonListeners : ConsumerBase
    {
        public CommonListeners() : base(RabbitMQProperties.ExchangeName, EventNames.Alert)
        {
        }

        public override async Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var handler = scope.ServiceProvider.GetService<CommonEventsHandlers>();
            var jObject = JObject.Parse(genericMessage.payload);
            var domain = jObject["DomainEventVM"].ToObject<DomainEventVM>();
            await handler.CommonCommandHandler(domain);
            return MessageAcknowledgement.Processed;
        }
    }
}
