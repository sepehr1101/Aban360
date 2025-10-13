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
    [Route("v1/service-link-payment-detail")]
    public class ServiceLinkPaymentDetailController : BaseController
    {
        private readonly IServiceLinkPaymentDetailHandler _serviceLinkPaymentDetail;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkPaymentDetailController(
            IServiceLinkPaymentDetailHandler serviceLinkPaymentDetail,
            IReportGenerator reportGenerator)
        {
            _serviceLinkPaymentDetail = serviceLinkPaymentDetail;
            _serviceLinkPaymentDetail.NotNull(nameof(_serviceLinkPaymentDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(PaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto> serviceLinkPaymentDetail = await _serviceLinkPaymentDetail.Handle(inputDto, cancellationToken);
            return Ok(serviceLinkPaymentDetail);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkPaymentDetail.Handle, CurrentUser, ReportLiterals.ServiceLinkPaymentDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(PaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 480;
            ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto> result = await _serviceLinkPaymentDetail.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
