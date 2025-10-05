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
    [Route("v1/without-sewage-request-summary-by-zone")]
    public class WithoutSewageRequestSummaryByZoneController : BaseController
    {
        private readonly IWithoutSewageRequestSummaryByZoneHandler _withoutSewageRequestSummaryByZoneHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutSewageRequestSummaryByZoneController(
            IWithoutSewageRequestSummaryByZoneHandler withoutSewageRequestSummaryByZoneHandler,
            IReportGenerator reportGenerator)
        {
            _withoutSewageRequestSummaryByZoneHandler = withoutSewageRequestSummaryByZoneHandler;
            _withoutSewageRequestSummaryByZoneHandler.NotNull(nameof(withoutSewageRequestSummaryByZoneHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutSewageRequestInputDto input, CancellationToken cancellationToken)
        {
            var result = await _withoutSewageRequestSummaryByZoneHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutSewageRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutSewageRequestSummaryByZoneHandler.Handle, CurrentUser, ReportLiterals.WithoutSewageRequestSummaryByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
