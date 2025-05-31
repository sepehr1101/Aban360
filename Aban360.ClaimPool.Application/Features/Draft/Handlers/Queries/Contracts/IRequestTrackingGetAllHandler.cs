using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts
{
    public interface IRequestTrackingGetAllHandler
    {
        Task<ICollection<RequestTrackingGetDto>> Handle(CancellationToken cancellationToken);
    }
}
