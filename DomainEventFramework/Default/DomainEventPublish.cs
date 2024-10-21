﻿using DomainEventFramework.Core;
using Messaging.Framework.RabbitMQ.Publisher;

namespace DomainEventFramework.Default
{
    internal class DomainEventPublish : IDomainEventPublish
    {
        private readonly IPublisher publisher;

        public DomainEventPublish(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public void Publish<T>(string eventName, T message, string userId = "") 
            where T: IDomainEventModel
        {
            string exchangeName = RabbitMQProperties.ExchangeName;
            if (string.IsNullOrWhiteSpace(userId))
                userId = RabbitMQProperties.DefaultUser;

            publisher.Publish(exchangeName, message, eventName, userId);
        }
    }
}