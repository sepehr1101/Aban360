using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-installation-summary")]
    public class SewageWaterInstallationSummaryController : BaseController
    {
        private readonly ISewageWaterInstallationSummaryHandler _sewageWaterInstallationSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterInstallationSummaryController(
            ISewageWaterInstallationSummaryHandler sewageWaterInstallationSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterInstallationSummaryHandler = sewageWaterInstallationSummaryHandler;
            _sewageWaterInstallationSummaryHandler.NotNull(nameof(sewageWaterInstallationSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationInputDto input, CancellationToken cancellationToken)
        {
            var result = await _sewageWaterInstallationSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterInstallationSummary: ReportLiterals.SewageInstallationSummary;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterInstallationSummaryHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
