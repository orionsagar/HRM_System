using Messaging.Framework.Common;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DomainEventFramework
{
    public class RabbitMQConsumer : ConsumerBase
    {
        public RabbitMQConsumer() :
            base("test", "testEvent")
        {
        }

        public override async Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var test = JsonConvert.DeserializeObject<string>(genericMessage.payload);
                Console.WriteLine(test);
            }

            return MessageAcknowledgement.Processed;
        }
    }
}
