using Messaging.Framework.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Framework.RabbitMQ.Consumer
{
    public class ConsumerRegistration
    {
        private static readonly List<ConsumerModel> consumerModels = new List<ConsumerModel>();
        private static readonly List<Type> consumerType = new List<Type>();
        private readonly ILogger<ConsumerRegistration> logger;
        private readonly IServiceProvider provider;

        public ConsumerRegistration(ILogger<ConsumerRegistration> logger, IServiceProvider provider)
        {
            this.logger = logger;
            this.provider = provider;
        }

        public static void RegisterConsumer<T>()
            where T : ConsumerBase
        {
            consumerType.Add(typeof(T));
        }

        public static List<ConsumerModel> GetConsumers() => consumerModels;

        public void Start(IModel channel, IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                consumerType.ForEach(type =>
                {
                    var model = (ConsumerBase)ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, type);
                    consumerModels.Add(new ConsumerModel(model.Exchange, model.EventName, model.HandleMessage, model.ServiceName));
                });
            }
            logger.LogInformation("starting consumer");
            if (consumerModels.Count < 1)
            {
                logger.LogWarning("No consumer registered.");
                return;
            }
            foreach (var model in consumerModels)
            {
                channel.ExchangeDeclare(exchange: model.getExchange(), type: "topic", true);
                string queueName = Common.Constants.InstanceName + "-" + model.getBindingKey();
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueBind(queue: queueName,
                                  exchange: model.getExchange(),
                                  routingKey: model.getBindingKey());

                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    GenericMessage genericMessage;
                    try
                    {
                        genericMessage = JsonConvert.DeserializeObject<GenericMessage>(message);
                    }
                    catch
                    {
                        var jObject = JObject.Parse(message);
                        string producer = jObject.GetValue("producer").ToString();
                        string eventName = jObject.GetValue("event_name").ToString();
                        string serviceName = jObject.GetValue("service_name").ToString();
                        string userId = jObject.GetValue("user_id").ToString();
                        var payload = jObject.GetValue("payload");
                        genericMessage = new GenericMessage(producer, eventName, payload.ToString(), userId, serviceName);
                    } 
                    var callback = model.getCallback();
                    var ack = await callback?.Invoke(genericMessage, provider);
                    switch (ack)
                    {
                        case MessageAcknowledgement.Processed:
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            break;
                        case MessageAcknowledgement.Error:
                            channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, false);
                            break;
                        case MessageAcknowledgement.Ignored:
                            channel.BasicReject(deliveryTag: ea.DeliveryTag, false);
                            break;
                    }

                };

                channel.BasicConsume(queueName, false, consumer);
            }
        }
    }
}
