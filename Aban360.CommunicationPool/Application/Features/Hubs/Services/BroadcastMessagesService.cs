using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using AutoMapper;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Services;
public interface IBroadcastMessagesService
{
    NotifyTextMessageOutput BroadcastTextMessageAsync(NotifyTextMessageInput input, CancellationToken cancellationToken);
}

internal sealed class BroadcastMessagesService : IBroadcastMessagesService
{
    public BroadcastMessagesService(IMapper mapper)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
    }

    private readonly IMapper _mapper;
    public NotifyTextMessageOutput BroadcastTextMessageAsync(NotifyTextMessageInput input, CancellationToken cancellationToken)
    {
        NotifyTextMessageOutput output = _mapper.Map<NotifyTextMessageOutput>(input);
        return output;
    }
}
