using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Libraries.EventSourcing;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;

namespace Common.Libraries.EventStore
{
    public class SubscriptionManager
    {
        private readonly ILogger<SubscriptionManager> _logger;

        readonly ICheckpointStore _checkpointStore;
        readonly string _name;
        readonly IEventStoreConnection _connection;
        readonly ISubscription[] _subscriptions;
        EventStoreAllCatchUpSubscription _subscription;

        public SubscriptionManager(
            IEventStoreConnection connection,
            ILogger<SubscriptionManager> logger,
            ICheckpointStore checkpointStore,
            string name,
            params ISubscription[] subscriptions)
        {
            _connection = connection;
            _checkpointStore = checkpointStore;
            _name = name;
            _logger = logger;
            _subscriptions = subscriptions;
        }

        public async Task Start()
        {
            var settings = new CatchUpSubscriptionSettings(
                2000, 500,
                _logger.IsEnabled(LogLevel.Debug),
                false, _name
            );

            _logger.LogDebug("Starting the projection manager...");

            var position = await _checkpointStore.GetCheckpoint();
            _logger.LogDebug("Retrieved the checkpoint: {checkpoint}", position);

            _subscription = _connection.SubscribeToAllFrom(
                GetPosition(),
                settings, 
                EventAppeared
            );
            _logger.LogDebug("Subscribed to $all stream");

            Position? GetPosition()
                => position.HasValue
                    ? new Position(position.Value, position.Value)
                    : AllCheckpoint.AllStart;
        }

        async Task EventAppeared(
            EventStoreCatchUpSubscription _,
            ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return;

            var @event = resolvedEvent.Deserialze();

            _logger.LogDebug("Projecting event {event}", @event.ToString());

            try
            {
                await Task.WhenAll(_subscriptions.Select(x => x.Project(@event)));

                await _checkpointStore.StoreCheckpoint(
                    // ReSharper disable once PossibleInvalidOperationException
                    resolvedEvent.OriginalPosition.Value.CommitPosition
                );
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Error occured when projecting the event {event}",
                    @event
                );
                throw;
            }
        }

        public void Stop() => _subscription.Stop();
    }
}