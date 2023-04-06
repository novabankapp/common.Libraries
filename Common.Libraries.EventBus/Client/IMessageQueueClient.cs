using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Libraries.EventBus.Client
{
    public interface IMessageQueueClient<T> where T : class
    {
        Task ConsumeMessage(string queueName, CancellationToken stoppingToken, Func<string, string, Task> ProcessEvent = null);
        Task PushMessage(string queueName, string routingKey, T message = null);

        Task Dispose();
    }
}