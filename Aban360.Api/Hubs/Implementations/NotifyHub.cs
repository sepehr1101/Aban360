using Aban360.CommunicationPool.Application.Features.Hubs.Services;
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
        private readonly IHubEventService _service;
        private readonly HubConnectionRegistry _connections;

        public NotifyHub(IHubEventService service, HubConnectionRegistry connections)
        {
            service.NotNull(nameof(service));
            _service = service;
            connections.NotNull(nameof(connections));
            _connections = connections;
        }

        public override async Task OnConnectedAsync()
        {
            await _service.CreateAsync(new HubEventCreateDto(Context.ConnectionId, CurrentUser.UserId), CancellationToken.None);
            _connections.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connections.Remove(Context.ConnectionId);
            try
            {
                await _service.UpdateAsync(new HubEventUpdateDto(Context.ConnectionId), CancellationToken.None);
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
