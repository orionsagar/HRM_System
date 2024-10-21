namespace Messaging.Framework.RabbitMQ.Publisher
{
    public interface IPublisher
    {
        void Publish(string exchangeName, object message, string eventName, string userId, string serviceName = "");
    }
}
