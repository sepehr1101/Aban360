using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IAssessmentByTrackIdGetHandler
    {
        Task<AssessmentDataOutputDto> Handle(Guid id, CancellationToken cancellationToken);
    }
}
