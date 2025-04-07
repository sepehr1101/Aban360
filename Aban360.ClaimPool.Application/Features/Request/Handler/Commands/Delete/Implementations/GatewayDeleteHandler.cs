using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Implementations
{
    internal sealed class GatewayDeleteHandler : IGatewayDeleteHandler
    {
        private readonly IGatewayCommandService _getewayCommandService;
        private readonly IGatewayQueryService _getewayQueryService;
        public GatewayDeleteHandler(
            IGatewayCommandService getewayCommandService,
            IGatewayQueryService getewayQueryService)
        {
            _getewayCommandService = getewayCommandService;
            _getewayCommandService.NotNull(nameof(_getewayCommandService));

            _getewayQueryService = getewayQueryService;
            _getewayQueryService.NotNull(nameof(_getewayQueryService));
        }

        public async Task Handle(GatewayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Gateway geteway = await _getewayQueryService.Get(deleteDto.Id);
            await _getewayCommandService.Remove(geteway);
        }
    }
}