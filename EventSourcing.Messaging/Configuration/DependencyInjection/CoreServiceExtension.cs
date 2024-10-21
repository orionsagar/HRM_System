using Messaging.Framework.RabbitMQ;
using Messaging.Framework.RabbitMQ.Consumer;
using Messaging.Framework.RabbitMQ.Publisher;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreServiceExtension
    {
        public static IMessagingBuilder AddCoreServices(this IMessagingBuilder builder)
        {
            builder.Services.AddSingleton<IBroker, Broker>();
            builder.Services.AddSingleton<IPublisher, DefaultPublisher>();
            builder.Services.AddSingleton<ConsumerRegistration>();
            return builder;
        }
    }
}
