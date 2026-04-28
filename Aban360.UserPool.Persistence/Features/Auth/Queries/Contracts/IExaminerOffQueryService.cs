using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IExaminerOffQueryService
    {
        Task<IEnumerable<AssessmentOffGetDto>> Get(int assessmentCode);
        Task<IEnumerable<AssessmentOffGetDto>> Get();
    }
}
