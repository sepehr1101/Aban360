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
    [Route("v1/sewage-water-request-noninstalled-summary-by-zone-grouped")]
    public class SewageWaterRequestNonInstalledSummaryByZoneGroupedController : BaseController
    {
        private readonly ISewageWaterRequestNonInstalledSummaryByZoneGroupedHandler _sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterRequestNonInstalledSummaryByZoneGroupedController(
            ISewageWaterRequestNonInstalledSummaryByZoneGroupedHandler sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler = sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler;
            _sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler.NotNull(nameof(sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, ReportOutput<SewageWaterRequestNonInstalledSummaryDataOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, ReportOutput<SewageWaterRequestNonInstalledSummaryDataOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>> result = await _sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterRequestNonInstalledInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterRequestNonInstalledSummary + ReportLiterals.ByZone : ReportLiterals.SewageRequestNonInstalledSummary + ReportLiterals.ByZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterRequestNonInstalledSummaryByZoneGroupedHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
