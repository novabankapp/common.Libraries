
using Common.Libraries.Saga.Data.Entities.Base;
using Common.Libraries.Services.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Nodes;

namespace Common.Libraries.Saga.Data.Entities
{
    public class SagaState : ISagaEntity, IEntity
    {
        public Guid ID { get; set; }

        [Column(TypeName = "jsonb")]
        public JsonNode Payload { get; set; }

        public int Version { get; set; }

        public string Type { get; set; }


        public string CurrentStep { get; set; }

        [Column(TypeName = "jsonb")]
        public JsonNode StepStatus { get; set; }

        public SagaStatus SagaStatus { get; set; }
        
        public DateTime DateCreated { get; set ; }
    }

    public enum SagaStatus
    {
        STARTED,
        ABORTING,
        ABORTED,
        COMPLETED

    }
}
