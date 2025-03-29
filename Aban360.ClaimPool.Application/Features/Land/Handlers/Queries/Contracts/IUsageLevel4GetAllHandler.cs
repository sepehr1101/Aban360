using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageLevel4GetAllHandler
    {
        Task<ICollection<UsageLevel4GetDto>> Handle(CancellationToken cancellationToken);
    }
}
