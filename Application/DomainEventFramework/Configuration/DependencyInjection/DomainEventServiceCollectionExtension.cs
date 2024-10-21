using Application.DomainEventFramework.Configuration.DependencyInjection;
using Application.DomainEventFramework.Core;
using Application.DomainEventFramework.Default;
using Application.DomainEventFramework.Listeners;
using Application.SupportiveBL.Context;
using Application.SupportiveBL.Email;
using Application.Tasks.DomainEvents.EventHandlers;
using Messaging.Configuration;
using Messaging.Framework.RabbitMQ.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application.DomainEventFramework.Configuration.DependencyInjection
{
    public static class DomainEventServiceCollectionExtension
    {
        private static IMessagingBuilder mBuilder;
        public static IDomainEventFrameworkBuilder AddDomainEventFramework(this IServiceCollection services, string instanceName, IConfiguration smtpConfigSection,
            Action<MessagingOptions> options = null,string defaultUser = "")
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
            builder
                .AddCoreServices(smtpConfigSection)
                .AddEventHandlers();
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

        private static IDomainEventFrameworkBuilder AddCoreServices(this IDomainEventFrameworkBuilder builder, IConfiguration smtpConfigSection)
        {
            builder.Services.Configure<SMTPOptions>(smtpConfigSection);
            builder.Services.AddScoped<IDomainEventPublish, DomainEventPublish>();
            builder.Services.AddScoped<IEmailSender, SMTPEmailSender>();
            return builder;
        }

        private static IDomainEventFrameworkBuilder AddEventHandlers(this IDomainEventFrameworkBuilder builder)
        {
            builder.Services.AddScoped<ClientEventHandlers>();
            builder.Services.AddScoped<InviteClientEventHandlers>();
            builder.Services.AddScoped<OrganisationEventHandlers>();
            builder.Services.AddScoped<GreetingsDomainEventsHandlers>();
            builder.Services.AddScoped<CommonEventsHandlers>();
            builder.Services.AddScoped<EmployeeEventsHandlers>();
            return builder;
        }
    }
}
