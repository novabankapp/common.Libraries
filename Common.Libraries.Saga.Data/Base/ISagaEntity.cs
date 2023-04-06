using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Libraries.Saga.Data.Entities.Base
{
    public interface ISagaEntity
    {
        [Key]
        public Guid ID { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
