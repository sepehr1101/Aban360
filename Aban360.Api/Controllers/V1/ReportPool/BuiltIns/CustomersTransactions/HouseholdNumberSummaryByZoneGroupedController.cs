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
    [Route("v1/household-number-summary-by-zone-grouped")]
    public class HouseholdNumberSummaryByZoneGroupedController : BaseController
    {
        private readonly IHouseholdNumberSummarybyZoneGroupedHandler _householdNumberSummaryByZoneGrouped;
        private readonly IReportGenerator _reportGenerator;
        public HouseholdNumberSummaryByZoneGroupedController(
            IHouseholdNumberSummarybyZoneGroupedHandler householdNumberSummaryByZoneGrouped,
            IReportGenerator reportGenerator)
        {
            _householdNumberSummaryByZoneGrouped = householdNumberSummaryByZoneGrouped;
            _householdNumberSummaryByZoneGrouped.NotNull(nameof(_householdNumberSummaryByZoneGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<HouseholdNumberHeaderOutputDto, ReportOutput<HouseholdNumberSummaryDataOutputDto, HouseholdNumberSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<HouseholdNumberHeaderOutputDto, ReportOutput<HouseholdNumberSummaryDataOutputDto, HouseholdNumberSummaryDataOutputDto>> householdNumberSummaryByZoneGrouped = await _householdNumberSummaryByZoneGrouped.Handle(inputDto, cancellationToken);
            return Ok(householdNumberSummaryByZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _householdNumberSummaryByZoneGrouped.Handle, CurrentUser, ReportLiterals.HouseholdNumberSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
