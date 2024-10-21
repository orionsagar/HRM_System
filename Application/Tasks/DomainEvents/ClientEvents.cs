using Application.DomainEventFramework.Core;

namespace Application.Tasks.DomainEvents
{
    public class ClientCreatedDomainEvent : IDomainEventModel
    {
        public ClientCreatedDomainEvent(int clientId, int userId)
        {
            ClientId = clientId;
            UserId = userId;
        }

        public int ClientId { get; }
        public int UserId { get; }
    }
}

