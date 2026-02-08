using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IExaminerTaskQueryService
    {
        Task<IEnumerable<GuidDictionary>> Get(int examinerCode);
        Task<IEnumerable<AssessmentLocationInfoWithSOutputDto>> GetLocationsInfo(IEnumerable<Guid> trackIds, int zoneId);
    }
}
