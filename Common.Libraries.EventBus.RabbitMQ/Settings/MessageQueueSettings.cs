using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Libraries.EventBus.RabbitMQ.Settings
{
    public class MessageQueueSettings : IMessageQueueSettings
    {
        public string HostName { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string Port { get; set; }
        public string Exchange { get; set ; }
    }
}
