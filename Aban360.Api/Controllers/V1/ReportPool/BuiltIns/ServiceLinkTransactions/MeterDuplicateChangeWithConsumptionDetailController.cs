using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/meter-duplicate-change-with-consumption-detail")]
    public class MeterDuplicateChangeWithConsumptionDetailController : BaseController
    {
        private readonly IMeterDuplicateChangeWithCustomerDetailHandler _meterDuplicateChangeSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterDuplicateChangeWithConsumptionDetailController(
            IMeterDuplicateChangeWithCustomerDetailHandler meterDuplicateChangeDetailHandler,
            IReportGenerator reportGenerator)
        {
            _meterDuplicateChangeSummaryHandler = meterDuplicateChangeDetailHandler;
            _meterDuplicateChangeSummaryHandler.NotNull(nameof(meterDuplicateChangeDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterDuplicateChangeWithCustomerDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Raw(MeterDuplicateChangeWithCustomerInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MeterDuplicateChangeWithCustomerDetailDataOutputDto> result = await _meterDuplicateChangeSummaryHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
