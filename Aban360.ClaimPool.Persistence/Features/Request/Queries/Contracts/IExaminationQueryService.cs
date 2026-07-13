using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IExaminationQueryService
    {
        Task<AssessmentGetDto> Get(int code);
        Task<AssessmentDataOutputDto> Get(Guid id);
        Task<AssessmentDataOutputDto> GetByTrackId(Guid id);
        Task<AssessmentDataOutputDto> GetLatestByTrackNumber(int trackNumber);
        Task<bool> HasResultByTrackId(Guid trackId);
        Task<int> GetWithoutResultInDate(string assessmentDateJalai, int assessmentCode);
        Task<IEnumerable<UnAssessmentDataOutputDto>> GetUnAssessment(IEnumerable<int> zoneIds);
    }
}
