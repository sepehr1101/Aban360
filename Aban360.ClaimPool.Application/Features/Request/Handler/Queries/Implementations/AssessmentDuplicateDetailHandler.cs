using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentDuplicateDetailHandler : IAssessmentDuplicateDetailHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private static string _title = ReportLiterals.AssessmentDuplicate;
        public AssessmentDuplicateDetailHandler(ITrackingQueryService trackingQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>> Handle(AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<AssessmentDuplicateDetailDataOutputDto> dataWithoutColour = await _trackingQueryService.Get(inputDto);
            IEnumerable<AssessmentDuplicateDetailDataOutputDto> data=SetColour(dataWithoutColour);
            AssessmentDuplicatHeaderOutputDto header = new()
            {
                FromDateJalali = inputDto.FromDateJalali,
                ToDateJalali = inputDto.ToDateJalali,
                RecordCount = dataWithoutColour?.Count() ?? 0,
                CustomerCount = dataWithoutColour?.DistinctBy(x => x.BillId)?.Count() ?? 0,
                Title = _title,
            };
            return new ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>(_title, header, data);
        }
        private IEnumerable<AssessmentDuplicateDetailDataOutputDto> SetColour(IEnumerable<AssessmentDuplicateDetailDataOutputDto> data)
        {
            string? tempVariable = null;
            bool isColour = false;
            foreach (var item in data)
            {
                if (tempVariable is not null && item.DuplicateItem != tempVariable)
                {
                    isColour = !isColour;
                }
                item.IsColour = isColour;
                tempVariable = item.DuplicateItem;
            }

            return data;
        }
    }
}