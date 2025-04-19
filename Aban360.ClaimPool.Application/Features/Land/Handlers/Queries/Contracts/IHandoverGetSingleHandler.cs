using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IHandoverGetSingleHandler
    {
        Task<HandoverGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
