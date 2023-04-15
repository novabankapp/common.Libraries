
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.EventBus.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
      where TEvent : Event
    {
        Task Handle(TEvent @event);


    }
    public interface IEventHandler
    {

    }
    public abstract class Event
    {
        public DateTime Timestamp { get; protected set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
