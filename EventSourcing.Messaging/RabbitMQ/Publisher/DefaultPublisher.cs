using Messaging.Framework.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Messaging.Framework.RabbitMQ.Publisher
{
    public class DefaultPublisher : IPublisher
    {
        private readonly IBroker broker;
        private readonly ILogger<DefaultPublisher> logger;

        public DefaultPublisher(IBroker broker, ILogger<DefaultPublisher> logger)
        {
            this.broker = broker;
            this.logger = logger;
        }
        public void Publish(string exchangeName, object payload, string eventName, string userId, string serviceName = "")
        {
            var serializedPayload = JsonConvert.SerializeObject(payload);
            var message = new GenericMessage(Common.Constants.InstanceName, eventName, serializedPayload, userId, serviceName);
            
            using var channel = broker.CreateChannel();
            channel.ExchangeDeclare(exchange: exchangeName, type: "topic", true);

            var data = JsonConvert.SerializeObject(message);

            string routingKey = eventName;
            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                routingKey = serviceName + "." + routingKey;
            }
            routingKey = exchangeName + "." + routingKey;

            var body = Encoding.UTF8.GetBytes(data);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
            logger.LogInformation("Message published to {exchange}: {body}", exchangeName, data);
        }
    }
}
