using System.Threading.Tasks;

namespace Common.Libraries.EventSourcing
{
    public interface ISubscription
    {
        Task Project(object @event);
    }
}