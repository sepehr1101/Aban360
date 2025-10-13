using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/without-bill-summary-by-zone-grouped")]
    public class WithoutBillSummaryByZoneGroupedController : BaseController
    {
        private readonly IWithoutBillSummaryByZoneGroupedHandler _withoutBillSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutBillSummaryByZoneGroupedController(
            IWithoutBillSummaryByZoneGroupedHandler withoutBillSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _withoutBillSummaryByZoneGroupedHandler = withoutBillSummaryByZoneGroupedHandler;
            _withoutBillSummaryByZoneGroupedHandler.NotNull(nameof(withoutBillSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutBillHeaderOutputDto, ReportOutput<WithoutBillSummaryDataOutputDto, WithoutBillSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WithoutBillHeaderOutputDto, ReportOutput<WithoutBillSummaryDataOutputDto, WithoutBillSummaryDataOutputDto>> withoutBillSummaryByZoneGrouped = await _withoutBillSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(withoutBillSummaryByZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutBillSummaryByZoneGroupedHandler.HandleFlat, CurrentUser, ReportLiterals.WithoutBillSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 213;
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto> nonPermanentBranch = await _withoutBillSummaryByZoneGroupedHandler.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(nonPermanentBranch, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
