using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Implementations
{
    public class GetewayDeleteHandler : IGetewayDeleteHandler
    {
        private readonly IGetewayCommandService _getewayCommandService;
        private readonly IGetewayQueryService _getewayQueryService;
        public GetewayDeleteHandler(
            IGetewayCommandService getewayCommandService,
            IGetewayQueryService getewayQueryService)
        {
            _getewayCommandService = getewayCommandService;
            _getewayCommandService.NotNull(nameof(_getewayCommandService));

            _getewayQueryService = getewayQueryService;
            _getewayQueryService.NotNull(nameof(_getewayQueryService));
        }

        public async Task Handle(GetewayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var geteway = await _getewayQueryService.Get(deleteDto.Id);
            if (geteway == null)
            {
                throw new InvalidDataException();
            }
            await _getewayCommandService.Remove(geteway);
        }
    }
}