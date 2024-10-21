using Messaging.Configuration;
using RabbitMQ.Client;
using System;

namespace Messaging.Framework.RabbitMQ
{
    public class Broker : IBroker
    {
        private readonly MessagingOptions messagingOptions;

        public Broker(MessagingOptions messagingOptions)
        {
            this.messagingOptions = messagingOptions ?? throw new ArgumentNullException(nameof(messagingOptions), "cannot be null");
            var factory = new ConnectionFactory
            {
                HostName = this.messagingOptions.HostUrl,
                UserName = this.messagingOptions.Username,
                Password = this.messagingOptions.Password,
                Port = this.messagingOptions.Port,
                VirtualHost = this.messagingOptions.VHost
            };

            this.Connection = factory.CreateConnection();
        }

        public IConnection Connection { get; }

        public IModel CreateChannel()
        {
            return Connection.CreateModel();
        }

        public void Dispose()
        {
            if (Connection != null && Connection.IsOpen)
                Connection.Close();
        }
    }
}
