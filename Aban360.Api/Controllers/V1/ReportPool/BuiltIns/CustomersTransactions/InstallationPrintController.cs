using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/installation-print")]
    public class InstallationPrintController : BaseController
    {
        private readonly IInstallationPrintHandler _installationPrintHandler;
        private readonly IReportGenerator _reportGenerator;
        public InstallationPrintController(
            IInstallationPrintHandler installationPrintHandler,
            IReportGenerator reportGenerator)
        {
            _installationPrintHandler = installationPrintHandler;
            _installationPrintHandler.NotNull(nameof(_installationPrintHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(InstallationPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto> result = await _installationPrintHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(InstallationPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2040;
            FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto> installationPrintHandler = await _installationPrintHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(installationPrintHandler, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
