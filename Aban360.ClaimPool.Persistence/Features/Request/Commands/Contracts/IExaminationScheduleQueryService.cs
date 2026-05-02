using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts
{
    public interface IExaminationScheduleQueryService
    {
        Task<IEnumerable<AssessmentScaduleGetDto>> Get(int zoneId, string readingNumber);
    }
}
