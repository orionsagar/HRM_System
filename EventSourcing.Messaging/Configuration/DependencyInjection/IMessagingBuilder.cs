// Copyright (c) Anick. All rights reserved.
// Author: Anick Chowdhury.

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Messaging builder Interface
    /// </summary>
    public interface IMessagingBuilder
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        IServiceCollection Services { get; }
    }
}
