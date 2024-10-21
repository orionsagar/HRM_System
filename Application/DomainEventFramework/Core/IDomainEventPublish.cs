using System.Collections.Generic;

namespace Application.DomainEventFramework.Core
{
    public interface IDomainEventPublish
    {
        void Publish<T>(string eventName, T message, string userId = "") where T: IDomainEventModel;
        //void Publish<T>(string eventName, List<T> message) where T: IDomainEventModel;
    }
}
