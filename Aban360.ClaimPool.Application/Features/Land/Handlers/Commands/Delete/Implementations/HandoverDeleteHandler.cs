using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class HandoverDeleteHandler : IHandoverDeleteHandler
    {
        private readonly IHandoverCommandService _handoverCommandService;
        private readonly IHandoverQueryService _handoverQueryService;
        public HandoverDeleteHandler(
            IHandoverCommandService handoverCommandService,
            IHandoverQueryService handoverQueryService)
        {
            _handoverCommandService = handoverCommandService;
            _handoverCommandService.NotNull(nameof(_handoverCommandService));

            _handoverQueryService = handoverQueryService;
            _handoverQueryService.NotNull(nameof(_handoverQueryService));
        }

        public async Task Handle(HandoverDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var handover = await _handoverQueryService.Get(deleteDto.Id);
            await _handoverCommandService.Remove(handover);
        }
    }
}
