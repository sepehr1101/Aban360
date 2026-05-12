using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class UnAssessmentGetAllHandler : IUnAssessmentGetAllHandler
    {
        private readonly IExaminationQueryService _examinationQueryService;
        private readonly ICommonZoneService _commonZoneService;
        const string _title = "نیازمند ارزیابی";
        public UnAssessmentGetAllHandler(
            IExaminationQueryService examinationQueryService,
            ICommonZoneService commonZoneService)
        {
            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>> Handle(IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> myZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            IEnumerable<UnAssessmentDataOutputDto> data = await _examinationQueryService.GetUnAssessment(myZoneIds);
            UnAssessmentHeaderOutputDto header = new(data?.Count() ?? 0, _title);

            return new ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>(_title, header, data);
        }
    }
}
