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
    [Route("v1/non-premanent-branch-summary-by-zone")]
    public class NonPermanentBranchSummaryByZoneController : BaseController
    {
        private readonly INonPermanentBranchSummaryByZoneHandler _nonPermanentBranchSummaryByZone;
        private readonly IReportGenerator _reportGenerator;
        public NonPermanentBranchSummaryByZoneController(INonPermanentBranchSummaryByZoneHandler nonPremanentBranchSummaryByZone,
            IReportGenerator reportGenerator)
        {
            _nonPermanentBranchSummaryByZone = nonPremanentBranchSummaryByZone;
            _nonPermanentBranchSummaryByZone.NotNull(nameof(_nonPermanentBranchSummaryByZone));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto> nonPremanentBranchSummaryByZone = await _nonPermanentBranchSummaryByZone.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranchSummaryByZone);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, NonPermanentBranchByUsageAndZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _nonPermanentBranchSummaryByZone.Handle, CurrentUser, ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
