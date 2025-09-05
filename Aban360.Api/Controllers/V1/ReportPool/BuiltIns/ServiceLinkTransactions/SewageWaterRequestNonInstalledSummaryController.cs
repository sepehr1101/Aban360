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
    [Route("v1/sewage-water-request-noninstalled-summary-by-usage")]
    public class SewageWaterRequestNonInstalledSummaryController : BaseController
    {
        private readonly ISewageWaterRequestNonInstalledSummaryHandler _sewageWaterRequestNonInstalledSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterRequestNonInstalledSummaryController(
            ISewageWaterRequestNonInstalledSummaryHandler sewageWaterRequestNonInstalledSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterRequestNonInstalledSummaryHandler = sewageWaterRequestNonInstalledSummaryHandler;
            _sewageWaterRequestNonInstalledSummaryHandler.NotNull(nameof(sewageWaterRequestNonInstalledSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken)
        {
            var result = await _sewageWaterRequestNonInstalledSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterRequestNonInstalledInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterRequestNonInstalledSummary+ReportLiterals.ByUsage : ReportLiterals.SewageRequestNonInstalledSummary + ReportLiterals.ByUsage;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterRequestNonInstalledSummaryHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
