using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IT64QueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
        Task<IEnumerable<AssessmentResultOutputDto>> GetAll();
    }
}
