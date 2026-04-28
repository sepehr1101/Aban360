using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class AssessmentOffGetAllHandler : IAssessmentOffGetAllHandler
    {
        private readonly IExaminerOffQueryService _examinerOffQueryService;
        public AssessmentOffGetAllHandler(
            IExaminerOffQueryService examinerOffQueryService)
        {
            _examinerOffQueryService = examinerOffQueryService;
            _examinerOffQueryService.NotNull(nameof(examinerOffQueryService));
        }

        public async Task<IEnumerable<AssessmentOffGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<AssessmentOffGetDto> data = await _examinerOffQueryService.Get();
            return data;
        }
    }
}
