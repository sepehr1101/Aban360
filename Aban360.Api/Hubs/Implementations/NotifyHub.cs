using Aban360.Api.Hubs.Contracts;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Microsoft.AspNetCore.SignalR;

namespace Aban360.Api.Hubs.Implementations
{
    public sealed class NotifyHub : Hub<INotifyHub>
    {
        private readonly IHubEventCreateHandler _eventCreateHandler;
        private readonly IHubEventUpdateHandler _eventUpdateHandler;
        public NotifyHub(IHubEventCreateHandler eventCreateHandler, IHubEventUpdateHandler eventUpdateHandler)
        {
            _eventCreateHandler = eventCreateHandler;
            _eventCreateHandler.NotNull(nameof(eventCreateHandler));

            _eventUpdateHandler = eventUpdateHandler;
            _eventUpdateHandler.NotNull(nameof(eventUpdateHandler));
        }

        public override async Task OnConnectedAsync()
        {
            //await _eventCreateHandler.Handle(new HubEventCreateDto(Context.ConnectionId, Guid.NewGuid()), CancellationToken.None);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //await _eventUpdateHandler.Handle(new HubEventUpdateDto(Context.ConnectionId), CancellationToken.None);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
