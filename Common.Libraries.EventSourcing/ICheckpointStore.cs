using System.Threading.Tasks;

namespace Common.Libraries.EventSourcing
{
    public interface ICheckpointStore
    {
        Task<long?> GetCheckpoint();
        Task StoreCheckpoint(long? checkpoint);
    }
}