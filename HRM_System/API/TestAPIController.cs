using Messaging.Framework.RabbitMQ.Publisher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UKHRM.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {
        private readonly IPublisher publisher;

        public TestAPIController(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        [HttpGet("rabbitmq")]
        public void TestRabbitMQ()
        {
            publisher.Publish("test", "test message", "testEvent", "user_id");
        }
    }
}
