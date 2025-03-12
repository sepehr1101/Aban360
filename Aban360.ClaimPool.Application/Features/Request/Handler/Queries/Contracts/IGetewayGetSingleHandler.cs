using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IGetewayGetSingleHandler
    {
        Task<GetewayGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}