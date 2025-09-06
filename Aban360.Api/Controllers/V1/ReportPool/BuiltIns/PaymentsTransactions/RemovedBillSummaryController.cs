using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/removed-bill-summary")]
    public class RemovedBillSummaryController : BaseController
    {
        private readonly IRemovedBillSummaryHandler _removedBillHandler;
        private readonly IReportGenerator _reportGenerator;
        public RemovedBillSummaryController(
            IRemovedBillSummaryHandler removedBillHandler,
            IReportGenerator reportGenerator)
        {
            _removedBillHandler = removedBillHandler;
            _removedBillHandler.NotNull(nameof(_removedBillHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(RemovedBillInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto> removedBill = await _removedBillHandler.Handle(inputDto, cancellationToken);
            return Ok(removedBill);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, RemovedBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _removedBillHandler.Handle, CurrentUser, ReportLiterals.RemovedBillSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
