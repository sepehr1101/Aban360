using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IFlatGetSingleHandler
    {
        Task<FlatGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
