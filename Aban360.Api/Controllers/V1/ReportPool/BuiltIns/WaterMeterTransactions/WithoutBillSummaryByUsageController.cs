using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/without-bill-summary-by-usage")]
    public class WithoutBillSummaryByUsageController : BaseController
    {
        private readonly IWithoutBillSummaryByUsageHandler _withoutBillSummaryByUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutBillSummaryByUsageController(
            IWithoutBillSummaryByUsageHandler withoutBillSummaryByUsageHandler,
            IReportGenerator reportGenerator)
        {
            _withoutBillSummaryByUsageHandler = withoutBillSummaryByUsageHandler;
            _withoutBillSummaryByUsageHandler.NotNull(nameof(withoutBillSummaryByUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto> withoutBillSummaryByUsage = await _withoutBillSummaryByUsageHandler.Handle(input, cancellationToken);
            return Ok(withoutBillSummaryByUsage);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutBillSummaryByUsageHandler.Handle, CurrentUser, ReportLiterals.WithoutBillSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }
    }
}
