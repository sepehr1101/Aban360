using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Implementations
{
    internal sealed class GatewayUpdateHandler : IGatewayUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IGatewayQueryService _getewayQueryService;
        public GatewayUpdateHandler(
            IMapper mapper,
            IGatewayQueryService getewayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _getewayQueryService = getewayQueryService;
            _getewayQueryService.NotNull(nameof(_getewayQueryService));
        }

        public async Task Handle(GetewayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Gateway gateway = await _getewayQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, gateway);
        }
    }
}