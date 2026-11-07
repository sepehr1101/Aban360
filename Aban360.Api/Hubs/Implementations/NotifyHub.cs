using Aban360.Api.Hubs.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Aban360.Api.Hubs.Implementations
{
    [Authorize]
    public sealed class NotifyHub : Hub<INotifyHub>
    {
        private readonly IHubEventCreateHandler _eventCreateHandler;
        private readonly IHubEventUpdateHandler _eventUpdateHandler;
        private readonly HubConnectionRegistry _connections;
        public NotifyHub(IHubEventCreateHandler eventCreateHandler, IHubEventUpdateHandler eventUpdateHandler,
            HubConnectionRegistry connections)
        {
            _eventCreateHandler = eventCreateHandler;
            _eventCreateHandler.NotNull(nameof(eventCreateHandler));

            _eventUpdateHandler = eventUpdateHandler;
            _eventUpdateHandler.NotNull(nameof(eventUpdateHandler));
            _connections = connections;
            _connections.NotNull(nameof(connections));
        }

        public override async Task OnConnectedAsync()
        {
            await _eventCreateHandler.Handle(new HubEventCreateDto(Context.ConnectionId, CurrentUser.UserId), CancellationToken.None);
            _connections.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connections.Remove(Context.ConnectionId);
            try
            {
                await _eventUpdateHandler.Handle(new HubEventUpdateDto(Context.ConnectionId), CancellationToken.None);
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }

        private IAppUser CurrentUser
        {
            get
            {
                return new AppUser(Context.User);
            }
        }
    }
}
