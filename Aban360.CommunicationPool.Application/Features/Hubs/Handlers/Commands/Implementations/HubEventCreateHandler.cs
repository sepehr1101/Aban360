using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using FluentValidation;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Implementations
{
    internal sealed class HubEventCreateHandler : IHubEventCreateHandler
    {
        private readonly IHubEventCreateService _hubEventQueryService;
        public HubEventCreateHandler(
            IHubEventCreateService hubEventQueryService)
        {
            _hubEventQueryService = hubEventQueryService;
            _hubEventQueryService.NotNull(nameof(hubEventQueryService));
        }

        public async Task Handle(HubEventCreateDto input, CancellationToken cancellationToken)
        {
            await _hubEventQueryService.Create(input);
        }
    }
}
