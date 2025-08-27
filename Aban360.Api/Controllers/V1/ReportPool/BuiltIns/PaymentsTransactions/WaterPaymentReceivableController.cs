using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/water-payment-receivable")]
    public class WaterPaymentReceivableController : BaseController
    {
        private readonly IWaterPaymentReceivableHandler _waterPaymentReceivable;
        private readonly IReportGenerator _reportGenerator;
        public WaterPaymentReceivableController(
            IWaterPaymentReceivableHandler waterPaymentReceivable,
            IReportGenerator reportGenerator)
        {
            _waterPaymentReceivable = waterPaymentReceivable;
            _waterPaymentReceivable.NotNull(nameof(_waterPaymentReceivable));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto> waterPaymentReceivable = await _waterPaymentReceivable.Handle(inputDto, cancellationToken);
            return Ok(waterPaymentReceivable);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterPaymentReceivable.Handle, CurrentUser, ReportLiterals.WaterPaymentReceivable, connectionId);
            return Ok(inputDto);
        }
    }
}
