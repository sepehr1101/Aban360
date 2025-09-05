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
    [Route("v1/sewage-water-request-summary-by-usage")]
    public class SewageWaterRequestSummaryController : BaseController
    {
        private readonly ISewageWaterRequestSummaryHandler _sewageWaterRequestSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterRequestSummaryController(
            ISewageWaterRequestSummaryHandler sewageWaterRequestSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterRequestSummaryHandler = sewageWaterRequestSummaryHandler;
            _sewageWaterRequestSummaryHandler.NotNull(nameof(sewageWaterRequestSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestInputDto input, CancellationToken cancellationToken)
        {
            var result = await _sewageWaterRequestSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterRequestSummary + ReportLiterals.ByUsage : ReportLiterals.SewageRequestSummary + ReportLiterals.ByUsage;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterRequestSummaryHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
