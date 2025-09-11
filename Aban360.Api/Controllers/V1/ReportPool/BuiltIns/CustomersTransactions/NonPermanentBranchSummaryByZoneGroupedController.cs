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
    [Route("v1/non-permanent-branch-summary-by-zone-grouped")]
    public class NonPermanentBranchSummaryByZoneGroupedController : BaseController
    {
        private readonly INonPermanentBranchSummaryByZoneGroupedHandler _nonPermanentBranchSummaryByZoneGrouped;
        private readonly IReportGenerator _reportGenerator;
        public NonPermanentBranchSummaryByZoneGroupedController(INonPermanentBranchSummaryByZoneGroupedHandler nonPremanentBranchSummaryByZoneGrouped,
            IReportGenerator reportGenerator)
        {
            _nonPermanentBranchSummaryByZoneGrouped = nonPremanentBranchSummaryByZoneGrouped;
            _nonPermanentBranchSummaryByZoneGrouped.NotNull(nameof(_nonPermanentBranchSummaryByZoneGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>> nonPremanentBranchSummaryByZoneGrouped = await _nonPermanentBranchSummaryByZoneGrouped.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranchSummaryByZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _nonPermanentBranchSummaryByZoneGrouped.Handle, CurrentUser, ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
