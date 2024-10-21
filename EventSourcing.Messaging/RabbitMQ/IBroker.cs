using RabbitMQ.Client;
using System;

namespace Messaging.Framework.RabbitMQ
{
    public interface IBroker : IDisposable
    {
        IModel CreateChannel();
    }
}
