using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Json;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentByTrackIdGetHandler : IAssessmentByTrackIdGetHandler
    {
        private readonly IExaminationQueryService _examinationQueryService;
        public AssessmentByTrackIdGetHandler(IExaminationQueryService examinationQueryService)
        {
            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));
        }

        public async Task<AssessmentDataOutputDto> Handle(Guid id, CancellationToken cancellationToken)
        {
            return await _examinationQueryService.GetByTrackId(id);
        }
    }
}
