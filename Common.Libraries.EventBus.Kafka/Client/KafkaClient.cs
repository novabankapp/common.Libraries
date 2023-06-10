using Common.Libraries.EventBus.Client;
using Common.Libraries.EventBus.Kafka.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Libraries.EventBus.Kafka.Client
{
    public class KafkaClient<T> : IMessageQueueClient<T> where T : class
    {
        private readonly IKafkaProducer<string, T> _producer;
        private readonly IKafkaConsumer<string, T> _consumer;

        public KafkaClient(IKafkaProducer<string, T> producer, IKafkaConsumer<string, T> consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        public async Task Dispose()
        {
            await Task.Run(() =>
            {
                _consumer.Close();
                _consumer.Dispose();
                
            });
        }

        public async Task ConsumeMessage(string topic, CancellationToken stoppingToken, Func<string, string, Task> ProcessEvent)
        {
            await _consumer.Consume(topic,stoppingToken);
        }

     

        public async Task PushMessage(string topic, string key, T message)
        {
             await _producer.ProduceAsync(topic, key,message);
        }
    }
}
