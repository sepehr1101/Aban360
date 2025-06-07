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
        public UnspecifiedWaterPaymentController(IUnspecifiedWaterPaymentHandler unspecifiedWaterPayment)
        {
            _unspecifiedWaterPayment = unspecifiedWaterPayment;
            _unspecifiedWaterPayment.NotNull(nameof(_unspecifiedWaterPayment));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnspecifiedPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto> unspecifiedWater = await _unspecifiedWaterPayment.Handle(inputDto, cancellationToken);
            return Ok(unspecifiedWater);
        }
    }
}
