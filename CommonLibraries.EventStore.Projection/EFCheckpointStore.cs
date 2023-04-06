using Common.Libraries.EventSourcing;
using Common.Libraries.Services.Entities;
using Common.Libraries.Services.Repositories;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.EventStore.Projection
{
    public class Checkpoint : IEntity
    {
        public string Id { get; set; }
        public long? Position { get; set; }
    }
    public class EFCheckpointStore : ICheckpointStore
    {
        private readonly IRepository<Checkpoint> _repository;
        readonly string _checkpointName;

        public EFCheckpointStore(IRepository<Checkpoint> repository, 
            string checkpointName)
        {
            _repository = repository;
            _checkpointName = checkpointName;
        }

        public async Task<long?> GetCheckpoint()
        {
            var checkPoint = await _repository.GetOneAsync(t => t.Id == _checkpointName);
            return checkPoint?.Position ?? AllCheckpoint.AllStart?.CommitPosition;
        }

        public async Task StoreCheckpoint(long? position)
        {
            var checkPoint = await _repository.GetOneAsync(t => t.Id == _checkpointName);
            if (checkPoint == null)
            {
                checkPoint = new Checkpoint
                {
                    Id = _checkpointName,
                    Position = position,
                };
                await _repository.AddAsync(checkPoint);
                return;
                
            }
            checkPoint.Position = position; 
            await _repository.UpdateAsync(checkPoint);

        }
    }
}
