using Aban360.ClaimPool.Domain.Features.Draff.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Draff.Handlers.Queries.Contracts
{
    public interface IRequestUserGetAllHandler
    {
        Task<ICollection<RequestUserQueryDto>> Handle(CancellationToken cancellationToken);
    }
}
