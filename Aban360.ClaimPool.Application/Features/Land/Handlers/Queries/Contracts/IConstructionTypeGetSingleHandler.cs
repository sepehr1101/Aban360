using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IConstructionTypeGetSingleHandler
    {
        Task<ConstructionTypeGetDto> Handle(short id,CancellationToken cancellationToken);
    }

}
