using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/water-payment-detail")]
    public class WaterPaymentDetailController : BaseController
    {
        private readonly IWaterPaymentDetailHandler _waterPaymentDetail;
        public WaterPaymentDetailController(IWaterPaymentDetailHandler waterPaymentDetail)
        {
            _waterPaymentDetail = waterPaymentDetail;
            _waterPaymentDetail.NotNull(nameof(_waterPaymentDetail));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterPaymentDetailHeaderOutputDto, WaterPaymentDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterPaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterPaymentDetailHeaderOutputDto, WaterPaymentDetailDataOutputDto> waterPaymentDetail = await _waterPaymentDetail.Handle(inputDto, cancellationToken);
            return Ok(waterPaymentDetail);
        }
    }
}
