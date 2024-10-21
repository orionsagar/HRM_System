using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class DomainEventFrameworkBuilder : IDomainEventFrameworkBuilder
    {
        public IServiceCollection Services { get; }
        public DomainEventFrameworkBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}
