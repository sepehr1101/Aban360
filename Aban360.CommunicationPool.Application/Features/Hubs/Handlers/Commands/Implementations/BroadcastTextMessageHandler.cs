using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using AutoMapper;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Implementations
{
    internal sealed class BroadcastTextMessageHandler : IBroadcastTextMessageHandler
    {
        private readonly IMapper _mapper;
        public BroadcastTextMessageHandler(IMapper mapper)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }
        public NotifyTextMessageOutput Handle(NotifyTextMessageInput input, CancellationToken cancellationToken)
        {
            NotifyTextMessageOutput output = _mapper.Map<NotifyTextMessageOutput>(input);
            return output;
        }
    }
}
