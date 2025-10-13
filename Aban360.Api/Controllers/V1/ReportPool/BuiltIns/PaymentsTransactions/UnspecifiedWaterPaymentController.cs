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
    [Route("v1/unspecified-water-payment")]
    public class UnspecifiedWaterPaymentController : BaseController
    {
        private readonly IUnspecifiedWaterPaymentHandler _unspecifiedWaterPayment;
        private readonly IReportGenerator _reportGenerator;
        public UnspecifiedWaterPaymentController(
            IUnspecifiedWaterPaymentHandler unspecifiedWaterPayment,
            IReportGenerator reportGenerator)
        {
            _unspecifiedWaterPayment = unspecifiedWaterPayment;
            _unspecifiedWaterPayment.NotNull(nameof(_unspecifiedWaterPayment));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnspecifiedPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto> unspecifiedWater = await _unspecifiedWaterPayment.Handle(inputDto, cancellationToken);
            return Ok(unspecifiedWater);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnspecifiedPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unspecifiedWaterPayment.Handle, CurrentUser, ReportLiterals.UnspecifiedWaterPayment, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(UnspecifiedPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 479;
            ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto> result = await _unspecifiedWaterPayment.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
