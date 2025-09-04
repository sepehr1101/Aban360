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
    [Route("v1/non-premanent-branch-summary-by-usage-and-zonee")]
    public class NonPermanentBranchSummaryByUsageAndZoneController : BaseController
    {
        private readonly INonPermanentBranchSummaryByUsageAndZoneHandler _nonPermanentBranchSummaryByUsageAndZone;
        private readonly IReportGenerator _reportGenerator;
        public NonPermanentBranchSummaryByUsageAndZoneController(INonPermanentBranchSummaryByUsageAndZoneHandler nonPremanentBranchSummaryByUsageAndZone,
            IReportGenerator reportGenerator)
        {
            _nonPermanentBranchSummaryByUsageAndZone = nonPremanentBranchSummaryByUsageAndZone;
            _nonPermanentBranchSummaryByUsageAndZone.NotNull(nameof(_nonPermanentBranchSummaryByUsageAndZone));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByUsageAndZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByUsageAndZoneDataOutputDto> nonPremanentBranchSummaryByUsageAndZone = await _nonPermanentBranchSummaryByUsageAndZone.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranchSummaryByUsageAndZone);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _nonPermanentBranchSummaryByUsageAndZone.Handle, CurrentUser, ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsageAndZone, connectionId);
            return Ok(inputDto);
        }
    }
}
