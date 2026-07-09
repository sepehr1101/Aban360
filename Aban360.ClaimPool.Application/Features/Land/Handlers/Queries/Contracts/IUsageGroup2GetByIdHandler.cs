using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageGroup2GetByIdHandler
    {
        Task<UsageGroup2GetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
