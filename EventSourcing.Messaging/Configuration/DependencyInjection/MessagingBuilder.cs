// Copyright (c) Anick. All rights reserved.
// Author: Anick Chowdhury.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Messaging.Configuration
{
    /// <summary>
    /// Messaging helper class for DI configuration
    /// </summary>
    public class MessagingBuilder : IMessagingBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public MessagingBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }


        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceCollection Services { get; }
    }
}
