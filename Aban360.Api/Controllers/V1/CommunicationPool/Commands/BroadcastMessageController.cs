using Aban360.Api.Hubs.Contracts;
using Aban360.Api.Hubs.Implementations;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Aban360.CommunicationPool.Application.Features.Hubs.Services;

namespace Aban360.Api.Controllers.V1.CommunicationPool.Commands
{
    [Route("v1/notification")]
    public class BroadcastMessageController : BaseController
    {
        private readonly IBroadcastMessagesService _broadcastHandler;
        private IHubContext<NotifyHub, INotifyHub> _notifyHub { get; }

        public BroadcastMessageController(IBroadcastMessagesService broadcastHandler, IHubContext<NotifyHub, INotifyHub> notifyHub)
        {
            _broadcastHandler = broadcastHandler;
            _broadcastHandler.NotNull(nameof(broadcastHandler));
            _notifyHub = notifyHub;
            _notifyHub.NotNull(nameof(_notifyHub));
        }

        [HttpPost]
        [Route("broadcast")]
        public async Task<IActionResult> NotifyUsers([FromBody] NotifyTextMessageInput notifyTextMessageInput, CancellationToken cancellationToken)
        {
            NotifyTextMessageOutput output = _broadcastHandler.BroadcastTextMessageAsync(notifyTextMessageInput, cancellationToken);
            await _notifyHub.Clients.All.BroadcastMessage(output);
            return Ok(notifyTextMessageInput);
        }
    }
}
