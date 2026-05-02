using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentByIdGetHandler : IAssessmentByIdGetHandler
    {
        private readonly IExaminationQueryService _examinationQueryService;
        public AssessmentByIdGetHandler(IExaminationQueryService examinationQueryService)
        {
            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));
        }

        public async Task<AssessmentDataOutputDto> Handle(Guid id, CancellationToken cancellationToken)
        {
            return await _examinationQueryService.Get(id);

        }
    }
}
