using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IGatewayGetSingleHandler
    {
        Task<GatewayGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}