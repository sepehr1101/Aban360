using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IHandoverGetAllHandler
    {
        Task<ICollection<HandoverGetDto>> Handle(CancellationToken cancellationToken);
    }
}
