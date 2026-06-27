using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/without-bill-summary-by-zone-usage")]
    public class WithoutBillSummaryByZoneUsageController : BaseController
    {
        private readonly IWithoutBillSummaryByZoneUsageHandler _withoutBillSummaryByZoneUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutBillSummaryByZoneUsageController(
            IWithoutBillSummaryByZoneUsageHandler withoutBillSummaryByZoneUsageHandler,
            IReportGenerator reportGenerator)
        {
            _withoutBillSummaryByZoneUsageHandler = withoutBillSummaryByZoneUsageHandler;
            _withoutBillSummaryByZoneUsageHandler.NotNull(nameof(withoutBillSummaryByZoneUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto> withoutBillSummaryByZoneGrouped = await _withoutBillSummaryByZoneUsageHandler.Handle(input, cancellationToken);
            return Ok(withoutBillSummaryByZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutBillSummaryByZoneUsageHandler.Handle, CurrentUser, ReportLiterals.WithoutBillSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.Handle);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 213;
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto> nonPermanentBranch = await _withoutBillSummaryByZoneUsageHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(nonPermanentBranch, cancellationToken, reportCode, true);
            return Ok(reportId);
        }
    }
}
