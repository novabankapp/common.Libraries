using Cassandra;
using Cassandra.Mapping;
using Common.Libraries.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.EventStore.Projection.Cassandra
{
    public class CassandraCheckPointStore : ICheckpointStore
    {
        readonly Func<ISession> _getSession;
        readonly string _checkpointName;
        private IMapper _mapper;
        public CassandraCheckPointStore(Func<ISession> getSession,
            string checkpointName)
        {
            _getSession = getSession;
            _checkpointName = checkpointName;
            _mapper = new Mapper(_getSession());
        }

        public async Task<long?> GetCheckpoint()
        {
            
            var checkpoint = await _mapper.FirstAsync<Checkpoint>("SELECT id, position FROM checkpoints WHERE SET  id =? ", _checkpointName);
            return checkpoint.Position;
        }

        public async Task StoreCheckpoint(long? position)
        {
            var checkpoint = await _mapper.FirstAsync<Checkpoint>("SELECT id, position FROM checkpoints WHERE SET  id =? ", _checkpointName);
            if (checkpoint == null)
            {
                checkpoint = new Checkpoint
                {
                    Id = _checkpointName
                };
                await _mapper.InsertAsync(checkpoint);
            }

            checkpoint.Position = position;
            await _mapper.UpdateAsync(checkpoint);
        }
    }
    class Checkpoint
    {
        public string Id { get; set; }
        public long? Position { get; set; }
    }
}
