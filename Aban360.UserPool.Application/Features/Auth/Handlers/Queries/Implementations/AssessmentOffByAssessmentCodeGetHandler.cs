using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class AssessmentOffByAssessmentCodeGetHandler : IAssessmentOffByAssessmentCodeGetHandler
    {
        private readonly IExaminerOffQueryService _examinerOffQueryService;
        private readonly IUserQueryService _userQueryService;
        public AssessmentOffByAssessmentCodeGetHandler(
            IExaminerOffQueryService examinerOffQueryService,
            IUserQueryService userQueryService)
        {
            _examinerOffQueryService = examinerOffQueryService;
            _examinerOffQueryService.NotNull(nameof(examinerOffQueryService));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task<IEnumerable<AssessmentOffGetDto>> Handle(int assessmentCode, CancellationToken cancellationToken)
        {
            IEnumerable<AssessmentOffGetDto> data = await _examinerOffQueryService.Get(assessmentCode);
            return data;
        }
    }
}
