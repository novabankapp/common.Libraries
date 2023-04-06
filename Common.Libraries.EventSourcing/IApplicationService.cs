using System.Threading.Tasks;

namespace Common.Libraries.EventSourcing
{
    public interface IApplicationService
    {
        Task Handle(object command);
    }
}