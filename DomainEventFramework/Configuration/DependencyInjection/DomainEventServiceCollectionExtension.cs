using DomainEventFramework.Configuration.DependencyInjection;
using DomainEventFramework.Core;
using DomainEventFramework.Default;
using Messaging.Configuration;
using Messaging.Framework.RabbitMQ.Consumer;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DomainEventServiceCollectionExtension
    {
        private static IMessagingBuilder mBuilder;
        public static IDomainEventFrameworkBuilder AddDomainEventFramework(this IServiceCollection services, string instanceName, Action<MessagingOptions> options = null, string defaultUser = "")
        {
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                throw new ArgumentNullException(nameof(instanceName), "must not be null");
            }

            var builder = new DomainEventFrameworkBuilder(services);
            if (options != null)
            {
                mBuilder = services.AddMessaging(instanceName, options);
            }
            builder.AddCoreServices();
            RabbitMQProperties.DefaultUser = defaultUser;
            RabbitMQProperties.ExchangeName = instanceName;

            return builder;
        }

        public static IDomainEventFrameworkBuilder AddEventListener<T>(this IDomainEventFrameworkBuilder builder)
            where T : ConsumerBase
        {
            if (mBuilder == null)
            {
                throw new NullReferenceException("Please add messaging options first.");
            }
            mBuilder.AddConsumer<T>();

            return builder;
        }

        public static IDomainEventFrameworkBuilder StartListening(this IDomainEventFrameworkBuilder builder)
        {
            if (mBuilder == null)
            {
                throw new NullReferenceException("Please add messaging options first.");
            }
            mBuilder.StartListening();

            return builder;
        }

        private static IDomainEventFrameworkBuilder AddCoreServices(this IDomainEventFrameworkBuilder builder)
        {
            builder.Services.AddScoped<IDomainEventPublish, DomainEventPublish>();
            return builder;
        }
    }
}
