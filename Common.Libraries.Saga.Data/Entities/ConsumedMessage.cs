
using Common.Libraries.Saga.Data.Entities.Base;
using Common.Libraries.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Libraries.Saga.Data.Entities
{
    public class ConsumedMessage : ISagaEntity, IEntity
    {
        public Guid ID { get; set ; }
        public DateTime DateCreated { get; set; }
    }
}
