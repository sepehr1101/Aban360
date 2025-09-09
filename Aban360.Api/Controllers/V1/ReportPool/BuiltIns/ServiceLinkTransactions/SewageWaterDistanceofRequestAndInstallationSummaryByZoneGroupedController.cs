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
    [Route("v1/sewage-water-distance-request-installation-summary-by-zone-grouped")]
    public class SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedController : BaseController
    {
        private readonly ISewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedController(
            ISewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler = sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler;
            _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.NotNull(nameof(sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>> result = await _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterDistanceofRequestAndInstallationByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterDistanceRequestInstallationSummaryByZone: ReportLiterals.SewageDistanceRequesteInstallationSummaryByZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
