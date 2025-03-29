using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageLevel2GetSingleHandler
    {
        Task<UsageLevel2GetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
