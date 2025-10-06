using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/without-sewage-request-summary-by-zone-grouped")]
    public class WithoutSewageRequestSummaryByZoneGroupedController : BaseController
    {
        private readonly IWithoutSewageRequestSummaryByZoneGroupedHandler _withoutSewageRequestSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutSewageRequestSummaryByZoneGroupedController(
            IWithoutSewageRequestSummaryByZoneGroupedHandler withoutSewageRequestSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _withoutSewageRequestSummaryByZoneGroupedHandler = withoutSewageRequestSummaryByZoneGroupedHandler;
            _withoutSewageRequestSummaryByZoneGroupedHandler.NotNull(nameof(withoutSewageRequestSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutSewageRequestHeaderOutputDto, ReportOutput<WithoutSewageRequestSummaryByZoneGroupedDataOutputDto, WithoutSewageRequestSummaryByZoneGroupedDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutSewageRequestInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WithoutSewageRequestHeaderOutputDto, ReportOutput<WithoutSewageRequestSummaryByZoneGroupedDataOutputDto, WithoutSewageRequestSummaryByZoneGroupedDataOutputDto>> result = await _withoutSewageRequestSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutSewageRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutSewageRequestSummaryByZoneGroupedHandler.HandleFlat, CurrentUser, ReportLiterals.WithoutSewageRequestSummaryByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }
    }
}
