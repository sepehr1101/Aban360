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
    [Route("v1/empty-unit-by-bill-summary-zone")]
    public class EmptyUnitByBillSummaryByZoneController : BaseController
    {
        private readonly IEmptyUnitByBillIdSummaryByZoneHandler _emptyUnitByBillZoneGrouping;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitByBillSummaryByZoneController(
            IEmptyUnitByBillIdSummaryByZoneHandler emptyUnitByBillZoneGrouping,
            IReportGenerator reportGenerator)
        {
            _emptyUnitByBillZoneGrouping = emptyUnitByBillZoneGrouping;
            _emptyUnitByBillZoneGrouping.NotNull(nameof(_emptyUnitByBillZoneGrouping));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneDataOutputDto> emptyUnit = await _emptyUnitByBillZoneGrouping.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnitByBillZoneGrouping.Handle, CurrentUser, ReportLiterals.EmptyUnitByBillSummary+ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
