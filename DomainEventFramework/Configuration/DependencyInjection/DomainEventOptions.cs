using Messaging.Configuration;
using System;

namespace DomainEventFramework.Configuration.DependencyInjection
{
    public class DomainEventOptions
    {
        public Action<MessagingOptions> MessagingOptions { get; set; }
        public string DefaultUser { get; set; } = string.Empty;
    }
}
