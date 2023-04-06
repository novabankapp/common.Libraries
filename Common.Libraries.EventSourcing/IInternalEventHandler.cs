namespace Common.Libraries.EventSourcing
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}