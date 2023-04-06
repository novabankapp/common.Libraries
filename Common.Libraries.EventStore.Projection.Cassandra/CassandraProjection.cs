using Cassandra;
using Common.Libraries.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.EventStore.Projection.Cassandra
{
    public class CassandraProjection : ISubscription
    {
        public CassandraProjection(
             GetSession getSession,
             Projector projector
            )
        {
            GetSession = getSession;
            _projector = projector;
        }
        GetSession GetSession { get; }
        readonly Projector _projector;
        public async Task Project(object @event)
        {
            using var session = GetSession();

            var handler = _projector(session, @event);
            if (handler == null) return;

            

            await handler();
            
        }
    }
    public delegate Func<Task> Projector(
           ISession session,
           object @event
       );
}
