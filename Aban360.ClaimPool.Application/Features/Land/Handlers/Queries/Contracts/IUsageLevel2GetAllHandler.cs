using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageLevel2GetAllHandler
    {
        Task<ICollection<UsageLevel2GetDto>> Handle(CancellationToken cancellationToken);
    }
}
