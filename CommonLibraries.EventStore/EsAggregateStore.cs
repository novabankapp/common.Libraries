using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Libraries.EventSourcing;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;

namespace Common.Libraries.EventStore
{
    public class EsAggregateStore : IAggregateStore
    {
        
        private readonly ILogger<EsAggregateStore> _logger;
        readonly IEventStoreConnection _connection;

        public EsAggregateStore(IEventStoreConnection connection,
            Microsoft.Extensions.Logging.ILogger<EsAggregateStore> logger)
        { 
            _connection = connection;
            _logger = logger;
        }

        public async Task Save<T>(T aggregate) where T : AggregateRoot
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var streamName = GetStreamName(aggregate);
            var changes = aggregate.GetChanges().ToArray();

            foreach (var change in changes)
               _logger.LogDebug("Persisting event {event}", change.ToString());

            await _connection.AppendEvents(streamName, aggregate.Version, changes);

            aggregate.ClearChanges();
        }

        public async Task<T> Load<T>(AggregateId<T> aggregateId)
            where T : AggregateRoot
        {
            if (aggregateId == null)
                throw new ArgumentNullException(nameof(aggregateId));

            var stream = GetStreamName(aggregateId);
            var aggregate = (T) Activator.CreateInstance(typeof(T), true);

            var page = await _connection.ReadStreamEventsForwardAsync(
                stream, 0, 1024, false
            );

            _logger.LogDebug("Loading events for the aggregate {aggregate}", aggregate.ToString());

            aggregate.Load(
                page.Events.Select(
                        resolvedEvent => resolvedEvent.Deserialze()
                    )
                    .ToArray()
            );

            return aggregate;
        }

        public async Task<bool> Exists<T>(AggregateId<T> aggregateId) 
            where T : AggregateRoot
        {
            var stream = GetStreamName(aggregateId);
            var result = await _connection.ReadEventAsync(stream, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }

        static string GetStreamName<T>(AggregateId<T> aggregateId) 
            where T : AggregateRoot 
            => $"{typeof(T).Name}-{aggregateId}";

        static string GetStreamName<T>(T aggregate)
            where T : AggregateRoot
            => $"{typeof(T).Name}-{aggregate.Id.ToString()}";
    }
}