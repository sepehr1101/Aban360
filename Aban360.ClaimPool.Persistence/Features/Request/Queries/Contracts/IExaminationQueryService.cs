using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IExaminationQueryService
    {
        Task<AssessmentGetDto> Get(int code);
        Task<bool> HasResultByTrackId(Guid trackId);
    }
}
