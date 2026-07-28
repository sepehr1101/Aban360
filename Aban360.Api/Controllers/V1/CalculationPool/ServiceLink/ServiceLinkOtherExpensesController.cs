using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link-other-expenses")]
    public class ServiceLinkOtherExpensesController : BaseController
    {
        private readonly IOtherExpensesInsertHandler _otherExpensesInsertHandler;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private int _taxItemId = 550;
        public ServiceLinkOtherExpensesController(
            IOtherExpensesInsertHandler otherExpensesInsertHandler,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient backgroundJobClient)
        {
            _otherExpensesInsertHandler = otherExpensesInsertHandler;
            _otherExpensesInsertHandler.NotNull(nameof(otherExpensesInsertHandler));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] OtherExpensesInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2540;
            ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto> result = await _otherExpensesInsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            string offeringTitle = result?.ReportData?.Where(r => r.OfferingId != _taxItemId)?.FirstOrDefault()?.OfferingTitle ?? string.Empty;
            string message = string.Format(SmsTemplates.ServiceLinkOtherExpensesInsert, offeringTitle, result?.ReportHeader?.TrackNumber, result?.ReportHeader?.FinalAmount, result?.ReportHeader?.BillId, result?.ReportHeader?.PaymentId, Environment.NewLine);
            //_backgroundJobClient.Enqueue(() => _smsOldHandler.Send(result.ReportHeader.MobileNumber, message, Guid.NewGuid()));
            _backgroundJobClient.Enqueue(() => _smsOldHandler.Send("09925306265", message, Guid.NewGuid()));
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
