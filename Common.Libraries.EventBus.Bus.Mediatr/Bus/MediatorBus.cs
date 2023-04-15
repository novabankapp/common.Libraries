

using Common.Libraries.EventBus.Bus;
using Common.Libraries.EventBus.Client;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.EventBus.Bus.Mediatr
{
    public sealed class MediatorBus : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorBus(IMediator mediator)
        {
            _mediator = mediator;     

            
        }

       

        public Task<CommandResult> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

       

    }

    
}
