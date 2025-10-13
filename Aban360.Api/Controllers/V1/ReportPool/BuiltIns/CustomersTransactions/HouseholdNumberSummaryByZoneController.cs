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
    [Route("v1/household-number-summary-by-zone")]
    public class HouseholdNumberSummaryByZoneController : BaseController
    {
        private readonly IHouseholdNumberSummarybyZoneHandler _householdNumberSummaryByZone;
        private readonly IReportGenerator _reportGenerator;
        public HouseholdNumberSummaryByZoneController(
            IHouseholdNumberSummarybyZoneHandler householdNumberSummaryByZone,
            IReportGenerator reportGenerator)
        {
            _householdNumberSummaryByZone = householdNumberSummaryByZone;
            _householdNumberSummaryByZone.NotNull(nameof(_householdNumberSummaryByZone));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto> householdNumberSummaryByZone = await _householdNumberSummaryByZone.Handle(inputDto, cancellationToken);
            return Ok(householdNumberSummaryByZone);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _householdNumberSummaryByZone.Handle, CurrentUser, ReportLiterals.HouseholdNumberSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 432;
            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto> result = await _householdNumberSummaryByZone.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
