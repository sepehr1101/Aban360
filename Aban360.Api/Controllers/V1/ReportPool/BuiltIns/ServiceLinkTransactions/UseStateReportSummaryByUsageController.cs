using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/use-state-report-summary-by-usage")]
    public class UseStateReportSummaryByUsageController : BaseController
    {
        private readonly IUseStateReportByUsageHandler _useStateReportSummaryByUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public UseStateReportSummaryByUsageController(
            IUseStateReportByUsageHandler useStateReportSummaryByUsageHandler,
            IReportGenerator reportGenerator)
        {
            _useStateReportSummaryByUsageHandler = useStateReportSummaryByUsageHandler;
            _useStateReportSummaryByUsageHandler.NotNull(nameof(_useStateReportSummaryByUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryByUsageDataOutputDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRaw(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryByUsageDataOutputDto> useStates = await _useStateReportSummaryByUsageHandler.Handle(inputDto, cancellationToken);
            return Ok(useStates);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _useStateReportSummaryByUsageHandler.Handle, CurrentUser, ReportLiterals.UseStateReport + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }
    }
}
