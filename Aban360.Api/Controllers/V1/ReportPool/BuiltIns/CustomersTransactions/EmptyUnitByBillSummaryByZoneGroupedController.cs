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
    [Route("v1/empty-unit-by-bill-summary-zone-grouped")]
    public class EmptyUnitByBillSummaryByZoneGroupedController : BaseController
    {
        private readonly IEmptyUnitByBillIdSummaryByZoneGroupedHandler _emptyUnitByBillZoneGroupedGrouping;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitByBillSummaryByZoneGroupedController(
            IEmptyUnitByBillIdSummaryByZoneGroupedHandler emptyUnitByBillZoneGroupedGrouping,
            IReportGenerator reportGenerator)
        {
            _emptyUnitByBillZoneGroupedGrouping = emptyUnitByBillZoneGroupedGrouping;
            _emptyUnitByBillZoneGroupedGrouping.NotNull(nameof(_emptyUnitByBillZoneGroupedGrouping));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>> emptyUnit = await _emptyUnitByBillZoneGroupedGrouping.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnitByBillZoneGroupedGrouping.Handle, CurrentUser, ReportLiterals.EmptyUnitByBillSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
