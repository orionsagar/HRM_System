using Messaging.Framework.RabbitMQ;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Messaging.Framework.BackgroundWorker
{
    public class RabbitMQSubscriptionService : BackgroundService
    {
        private readonly IServiceProvider provider;
        private readonly ILogger<RabbitMQSubscriptionService> logger;
        private readonly ConsumerRegistration consumer;
        private readonly IBroker broker;
        private IModel channel;

        public RabbitMQSubscriptionService(IServiceProvider provider, ILogger<RabbitMQSubscriptionService> logger, ConsumerRegistration consumer, IBroker broker)
        {
            this.provider = provider;
            this.logger = logger;
            this.consumer = consumer;
            this.broker = broker;
            channel = this.broker.CreateChannel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            provider.GetService(typeof(ConsumerBase));
            consumer.Start(channel, provider);

            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            if (channel.IsOpen)
                channel.Close();
            broker.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
