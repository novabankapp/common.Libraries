using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Libraries.Services.Exceptions
{
    public class EntityNotFoundException : Exception
    {



        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
