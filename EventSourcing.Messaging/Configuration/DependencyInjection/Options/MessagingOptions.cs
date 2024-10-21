// Copyright (c) Anick. All rights reserved.
// Author: Anick Chowdhury.

namespace Messaging.Configuration
{
    /// <summary>
    /// The Messagingoptions class is the top level container for all configuration settings of Messaging.
    /// </summary>
    public class MessagingOptions
    {
        public string HostUrl { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VHost { get; set; } = "/";
    }
}
