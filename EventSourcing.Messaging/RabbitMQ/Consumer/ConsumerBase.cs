using Messaging.Framework.Common;
using System;
using System.Threading.Tasks;

namespace Messaging.Framework.RabbitMQ.Consumer
{
    public abstract class ConsumerBase
    {
        public string Exchange { get; }

        public string EventName { get; }

        public string ServiceName { get; set; }

        public ConsumerBase(string exchange, string eventName)
        {
            Exchange = exchange;
            EventName = eventName;
        }

        public abstract Task<MessageAcknowledgement> HandleMessage(GenericMessage genericMessage, IServiceProvider provider);
    }
}
