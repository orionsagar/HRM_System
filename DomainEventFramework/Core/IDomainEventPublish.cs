namespace DomainEventFramework.Core
{
    public interface IDomainEventPublish
    {
        void Publish<T>(string eventName, T message, string userId = "") where T: IDomainEventModel;
    }
}
