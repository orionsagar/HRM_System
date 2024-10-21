// Copyright (c) Anick. All rights reserved.
// Author: Anick Chowdhury.

using Messaging.Framework.Common;
using Messaging.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DI extension methods for adding messaging
    /// </summary>
    public static class MessagingServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        private static IMessagingBuilder AddMessagingBuilder(this IServiceCollection services, Action<MessagingOptions> options = null)
        {
            var builder = new MessagingBuilder(services);
            if (options != null)
                services.Configure(options);
            services.AddOptions();
            services.AddSingleton(resolve => resolve.GetRequiredService<IOptions<MessagingOptions>>().Value);

            builder.AddCoreServices();
            return builder;
        }

        /// <summary>
        /// Adds Messaging.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IMessagingBuilder AddMessaging(this IServiceCollection services, string instanceName, Action<MessagingOptions> options = null)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                throw new ArgumentNullException(nameof(instanceName), "must not be null");
            }
            Constants.InstanceName = instanceName;
            var builder = services.AddMessagingBuilder(options);

            return builder;
        }
    }
}
