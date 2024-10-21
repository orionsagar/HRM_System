using Messaging.Framework.BackgroundWorker;
using Messaging.Framework.RabbitMQ.Consumer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsumerExtension
    {
        public static IMessagingBuilder AddConsumer<T>(this IMessagingBuilder builder)
            where T : ConsumerBase
        {

            ConsumerRegistration.RegisterConsumer<T>();
            return builder;
        }

        public static IMessagingBuilder StartListening(this IMessagingBuilder builder)
        {
            builder.Services.AddHostedService<RabbitMQSubscriptionService>();

            return builder;
        }
    }
}
