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
    [Route("v1/empty-unit-by-bill-summary-usage")]
    public class EmptyUnitByBillSummaryByUsageController : BaseController
    {
        private readonly IEmptyUnitByBillIdSummaryByUsageHandler _emptyUnitByBillUsageGrouping;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitByBillSummaryByUsageController(
            IEmptyUnitByBillIdSummaryByUsageHandler emptyUnitByBillUsageGrouping,
            IReportGenerator reportGenerator)
        {
            _emptyUnitByBillUsageGrouping = emptyUnitByBillUsageGrouping;
            _emptyUnitByBillUsageGrouping.NotNull(nameof(_emptyUnitByBillUsageGrouping));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto> emptyUnit = await _emptyUnitByBillUsageGrouping.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnitByBillUsageGrouping.Handle, CurrentUser, ReportLiterals.EmptyUnitByBillSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }
    }
}
