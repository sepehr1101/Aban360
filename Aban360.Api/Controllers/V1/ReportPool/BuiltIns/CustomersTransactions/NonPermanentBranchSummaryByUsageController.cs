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
    [Route("v1/non-permanent-branch-summary-by-usage")]
    public class NonPermanentBranchSummaryByUsageController : BaseController
    {
        private readonly INonPermanentBranchSummaryByUsageHandler _nonPermanentBranchSummaryByUsage;
        private readonly IReportGenerator _reportGenerator;
        public NonPermanentBranchSummaryByUsageController(INonPermanentBranchSummaryByUsageHandler nonPremanentBranchSummaryByUsage,
            IReportGenerator reportGenerator)
        {
            _nonPermanentBranchSummaryByUsage = nonPremanentBranchSummaryByUsage;
            _nonPermanentBranchSummaryByUsage.NotNull(nameof(_nonPermanentBranchSummaryByUsage));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByUsageDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByUsageDataOutputDto> nonPremanentBranchSummaryByUsage = await _nonPermanentBranchSummaryByUsage.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranchSummaryByUsage);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _nonPermanentBranchSummaryByUsage.Handle, CurrentUser, ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }
    }
}
