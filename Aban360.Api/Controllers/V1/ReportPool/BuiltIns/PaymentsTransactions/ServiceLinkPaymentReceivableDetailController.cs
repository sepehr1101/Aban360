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
    [Route("v1/service-link-payment-receivable-detail")]
    public class ServiceLinkPaymentReceivableDetailController : BaseController
    {
        private readonly IServiceLinkPaymentReceivableDetailHandler _serviceLinkPaymentReceivable;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkPaymentReceivableDetailController(
            IServiceLinkPaymentReceivableDetailHandler serviceLinkPaymentReceivable,
            IReportGenerator reportGenerator)
        {
            _serviceLinkPaymentReceivable = serviceLinkPaymentReceivable;
            _serviceLinkPaymentReceivable.NotNull(nameof(_serviceLinkPaymentReceivable));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto> waterPaymentReceivable = await _serviceLinkPaymentReceivable.Handle(inputDto, cancellationToken);
            return Ok(waterPaymentReceivable);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkPaymentReceivable.Handle, CurrentUser, ReportLiterals.ServiceLinkPaymentReceivableDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ServiceLinkPaymentReceivableInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 580;
            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto> result = await _serviceLinkPaymentReceivable.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
