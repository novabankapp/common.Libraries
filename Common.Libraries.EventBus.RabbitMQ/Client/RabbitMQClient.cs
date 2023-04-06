using Common.Libraries.EventBus.Client;
using Common.Libraries.EventBus.RabbitMQ.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Libraries.EventBus.RabbitMQ.Client
{
    public class RabbitMQClient<T> : IMessageQueueClient<T> where T : class
    {
        private readonly IModel _channel;
        private readonly ILogger _logger;
        private readonly IMessageQueueSettings _options;
        public RabbitMQClient(IMessageQueueSettings options, ILogger<RabbitMQClient<T>> logger)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = options.HostName,
                    UserName = options.Username,
                    Password = options.Password,
                    Port = int.Parse(options.Port)
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
                _options = options;
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQ initialization failed");
            }
            _logger = logger;
        }
        private Func<string, string, Task> ProcessEventToConsume;
        public virtual async Task ConsumeMessage(string queueName, CancellationToken stoppingToken, Func<string, string, Task> ProcessEvent)
        {
            _channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            ProcessEventToConsume = ProcessEvent;
            consumer.Received += ConsumerReceived;
            await Task.Run(() => _channel.BasicConsume(queueName, true, consumer));
        }
        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                await ProcessEventToConsume(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(-1, ex, "Error consuming message");
            }
        }
        public virtual async Task PushMessage(string queueName, string routingKey, T message)
        {
            _logger.LogInformation($"Push Message, routingKey:{routingKey}");
            _channel.QueueDeclare(queue: queueName,
                 durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            await Task.Run(() => _channel.BasicPublish(exchange: _options.Exchange, routingKey: routingKey, basicProperties: null, body: body));
        }

      

        public async Task Dispose()
        {
            await Task.Run(() => _channel.Dispose());
        }
    }
}
