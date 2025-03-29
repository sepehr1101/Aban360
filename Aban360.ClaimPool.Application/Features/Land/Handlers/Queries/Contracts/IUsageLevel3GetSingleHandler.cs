using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageLevel3GetSingleHandler
    {
        Task<UsageLevel3GetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
