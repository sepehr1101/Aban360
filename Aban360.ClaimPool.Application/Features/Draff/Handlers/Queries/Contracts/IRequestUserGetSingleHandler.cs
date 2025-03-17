using Aban360.ClaimPool.Domain.Features.Draff.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Draff.Handlers.Queries.Contracts
{
    public interface IRequestUserGetSingleHandler
    {
        Task<RequestUserQueryDto> Handle(short id, CancellationToken cancellationToken);
    }
}
