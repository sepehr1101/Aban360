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
    [Route("v1/invalid-payment")]
    public class InvalidPaymentController : BaseController
    {
        private readonly IInvalidPaymentHandler _InvalidPayment;
        private readonly IReportGenerator _reportGenerator;
        public InvalidPaymentController(
            IInvalidPaymentHandler InvalidPayment,
            IReportGenerator reportGenerator)
        {
            _InvalidPayment = InvalidPayment;
            _InvalidPayment.NotNull(nameof(_InvalidPayment));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(InvalidPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto> InvalidPayment = await _InvalidPayment.Handle(inputDto, cancellationToken);
            return Ok(InvalidPayment);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, InvalidPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _InvalidPayment.Handle, CurrentUser, ReportLiterals.InvalidPayment, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(InvalidPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 560;
            ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto> result = await _InvalidPayment.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
