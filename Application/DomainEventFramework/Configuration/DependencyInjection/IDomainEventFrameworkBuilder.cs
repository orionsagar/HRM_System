namespace Microsoft.Extensions.DependencyInjection
{
    public interface IDomainEventFrameworkBuilder
    {
        public IServiceCollection Services { get; }
    }
}
