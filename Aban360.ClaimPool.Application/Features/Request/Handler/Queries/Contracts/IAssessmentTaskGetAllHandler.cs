using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IAssessmentTaskGetAllHandler
    {
        Task<AssessmentTasksOutputDto> Handle(int examinerCode, CancellationToken cancellationToken);
    }
}