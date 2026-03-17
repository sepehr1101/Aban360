using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IMotherChildGetByTrackNumberHandler
    {
        Task<MotherChildOutputDto> Handle(int trackNumber, CancellationToken cancellationToken);
    }
}
