using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace Common.Libraries.EventStore
{
    public class EventStoreService : IHostedService
    {
        readonly IEventStoreConnection _esConnection;
        readonly IEnumerable<SubscriptionManager> _subscriptionManagers;

        public EventStoreService(
            IEventStoreConnection esConnection,
            IEnumerable<SubscriptionManager> subscriptionManagers)
        {
            _esConnection = esConnection;
            _subscriptionManagers = subscriptionManagers;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _esConnection.ConnectAsync();

            await Task.WhenAll(
                _subscriptionManagers
                    .Select(projection => projection.Start())
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(var subscriptionManager in _subscriptionManagers)
            {
                subscriptionManager.Stop();
            }
            
            _esConnection.Close();
            return Task.CompletedTask;
        }
    }
}