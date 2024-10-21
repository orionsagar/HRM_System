namespace Microsoft.Extensions.DependencyInjection
{
    public class DomainEventFrameworkBuilder : IDomainEventFrameworkBuilder
    {
        public DomainEventFrameworkBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
