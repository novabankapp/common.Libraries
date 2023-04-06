using Common.Libraries.EventSourcing;
using Common.Libraries.Services.Entities;
using Common.Libraries.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.EventStore.Projection
{
    public class EFDbProjection<T> : ISubscription where T : class, IEntity
    {
        readonly Projector _projector;
        readonly IRepository<T> _repository;

        public EFDbProjection(EFDbProjection<T>.Projector projector, 
            IRepository<T> repository)
        {
            _projector = projector;
            _repository = repository;
        }

        public async Task Project(object @event)
        {
            var handler = _projector(_repository, @event);
            if (handler == null) return;
            await handler();
            
        }
        public delegate Func<Task> Projector(
            IRepository<T> repository,
            object @event
        );
    }
}
