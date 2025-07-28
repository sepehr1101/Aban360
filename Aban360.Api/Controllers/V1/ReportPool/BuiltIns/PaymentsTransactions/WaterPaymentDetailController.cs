using Aban360.Api.Cronjobs;
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
        private readonly IReportGenerator _reportGenerator;
        public WaterPaymentDetailController(
            IWaterPaymentDetailHandler waterPaymentDetail,
            IReportGenerator reportGenerator)
        {
            _waterPaymentDetail = waterPaymentDetail;
            _waterPaymentDetail.NotNull(nameof(_waterPaymentDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(PaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto> waterPaymentDetail = await _waterPaymentDetail.Handle(inputDto, cancellationToken);
            return Ok(waterPaymentDetail);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterPaymentDetail.Handle, CurrentUser, ReportLiterals.WaterPaymentDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
