using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/install-gt-request")]
    public class InstallGtReqeustController : BaseController
    {
        private readonly IInstallGtRequestHandler _installGtRequestHandler;
        private readonly IReportGenerator _reportGenerator;
        public InstallGtReqeustController(
            IInstallGtRequestHandler installGtRequestHandler,
            IReportGenerator reportGenerator)
        {
            _installGtRequestHandler = installGtRequestHandler;
            _installGtRequestHandler.NotNull(nameof(installGtRequestHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Raw(InstallGtRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto> result = await _installGtRequestHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> Excel(string connectionId, InstallGtRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _installGtRequestHandler.Handle, CurrentUser, ReportLiterals.WaterInstallGtRequest,connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(InstallGtRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 289;
            ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto> DeletionStateChangeHistory = await _installGtRequestHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(DeletionStateChangeHistory, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
