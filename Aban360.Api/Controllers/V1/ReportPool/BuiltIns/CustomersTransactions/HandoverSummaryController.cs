using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/handover-summary")]
    public class HandoverSummaryController : BaseController
    {
        private readonly IHandoverSummaryHandler _handoverSummary;
        private readonly IReportGenerator _reportGenerator;
        public HandoverSummaryController(
            IHandoverSummaryHandler handoverSummary,
            IReportGenerator reportGenerator)
        {
            _handoverSummary = handoverSummary;
            _handoverSummary.NotNull(nameof(_handoverSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(HandoverInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto> HandoverSummary = await _handoverSummary.Handle(inputDto, cancellationToken);
            return Ok(HandoverSummary);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, HandoverInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _handoverSummary.Handle, CurrentUser, ReportLiterals.HandoverSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
