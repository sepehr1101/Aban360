using Aban360.Api.Hubs.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Aban360.Api.Hubs.Implementations
{
    public sealed class NotifyHub : Hub<INotifyHub>
    {
        public override Task OnConnectedAsync()
        {
            //insert into eventhub
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //update eventhub
            return base.OnDisconnectedAsync(exception);
        }
    }
}
