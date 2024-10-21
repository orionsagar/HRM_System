using System;

namespace Messaging.Framework.Common
{
    public class GenericMessage
    {
        public GenericMessage(string producer, string event_name, string payload, string user_id = "", string service_name = "")
        {
            this.producer = producer ?? throw new ArgumentException($"'{nameof(producer)}' cannot be null or empty", nameof(producer));
            this.event_name = event_name ?? throw new ArgumentException($"'{nameof(event_name)}' cannot be null or empty", nameof(event_name)); ;
            this.user_id = user_id;
            this.service_name = service_name;
            this.payload = payload;
        }
        public string producer { get; }
        public string user_id { get; }
        public string event_name { get; }
        public string service_name { get; }
        public string payload { get; private set; }
    }
}
