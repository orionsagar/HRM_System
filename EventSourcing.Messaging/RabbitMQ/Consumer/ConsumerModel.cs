using Messaging.Framework.Common;
using System;
using System.Threading.Tasks;

namespace Messaging.Framework.RabbitMQ.Consumer
{
    public class ConsumerModel
    {
        private readonly string exchange;
        private readonly string eventName;
        private readonly Func<GenericMessage, IServiceProvider, Task<MessageAcknowledgement>> callback;
        private readonly string service;

        public ConsumerModel(string exchange, string eventName, Func<GenericMessage, IServiceProvider, Task<MessageAcknowledgement>> callback, string service = "")
        {
            this.exchange = exchange;
            this.eventName = eventName;
            this.callback = callback;
            this.service = service;
        }

        public string getExchange()
        {
            return exchange;
        }

        public string getBindingKey()
        {
            string routingKey = eventName;
            if (!string.IsNullOrWhiteSpace(service))
            {
                routingKey = service + "." + routingKey;
            }
            routingKey = exchange + "." + routingKey;
            return routingKey;
        }

        public Func<GenericMessage, IServiceProvider, Task<MessageAcknowledgement>> getCallback()
        {
            return callback;
        }
    }
}
