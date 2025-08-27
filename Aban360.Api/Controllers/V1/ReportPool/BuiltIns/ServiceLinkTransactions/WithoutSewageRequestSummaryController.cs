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
    [Route("v1/without-sewage-request-summary")]
    public class WithoutSewageRequestSummaryController : BaseController
    {
        private readonly IWithoutSewageRequestSummaryHandler _withoutSewageRequestSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutSewageRequestSummaryController(
            IWithoutSewageRequestSummaryHandler withoutSewageRequestSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _withoutSewageRequestSummaryHandler = withoutSewageRequestSummaryHandler;
            _withoutSewageRequestSummaryHandler.NotNull(nameof(withoutSewageRequestSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutSewageRequestInputDto input, CancellationToken cancellationToken)
        {
            var result = await _withoutSewageRequestSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutSewageRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutSewageRequestSummaryHandler.Handle, CurrentUser, ReportLiterals.WithoutSewageRequestSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
