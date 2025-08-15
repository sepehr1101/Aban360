using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/non-premanent-branch-summary")]
    public class NonPermanentBranchSummaryController : BaseController
    {
        private readonly INonPermanentBranchSummaryHandler _nonPermanentBranchSummary;
        private readonly IReportGenerator _reportGenerator;
        public NonPermanentBranchSummaryController(INonPermanentBranchSummaryHandler nonPremanentBranchSummary,
            IReportGenerator reportGenerator)
        {
            _nonPermanentBranchSummary = nonPremanentBranchSummary;
            _nonPermanentBranchSummary.NotNull(nameof(_nonPermanentBranchSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto> nonPremanentBranchSummary = await _nonPermanentBranchSummary.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranchSummary);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, NonPermanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _nonPermanentBranchSummary.Handle, CurrentUser, ReportLiterals.NonPermanentBranchSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
