using Aban360.Api.Cronjobs;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/service-link")]
    public class ServiceLinkManagerController : BaseController
    {
        private readonly IConnectDisconnectPrintHandler _connectDisconnectPrint;
        private readonly IReportGenerator _reportGenerator;
        private readonly ISmsOldHandler _smsHandler;
        private readonly IBackgroundJobClient _jobClient;
        private readonly ICustomerUpdateHandler _customerUpdateHandler;
        string successfullyDone = "با موفقیت انجام شد";
        int _disconnectState = 5;
        int _connectState = 0;
        public ServiceLinkManagerController(
            IConnectDisconnectPrintHandler connectDisconnectPrint,
            IReportGenerator reportGenerator,
            ISmsOldHandler smsHandler,
            IBackgroundJobClient jobClient,
            ICustomerUpdateHandler customerUpdateHandler)
        {
            _connectDisconnectPrint = connectDisconnectPrint;
            _connectDisconnectPrint.NotNull(nameof(connectDisconnectPrint));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));

            _smsHandler = smsHandler;
            _smsHandler.NotNull(nameof(smsHandler));

            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));

            _customerUpdateHandler = customerUpdateHandler;
            _customerUpdateHandler.NotNull(nameof(customerUpdateHandler));
        }


        [HttpPost]
        [Route("connect-command")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConnectCommandSti(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2070;
            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await _connectDisconnectPrint.Handle(inputDto, CurrentUser, true, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode, true);
            _jobClient.Enqueue(() => _smsHandler.Send("09925306265", result.ReportHeader.MessageText, Guid.NewGuid()));
            //result.ReportData.FirstOrDefault().MobileNumber

            return Ok(reportId);
        }

        [HttpPost]
        [Route("disconnect-command")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDisconnectCommandSti(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2071;
            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await _connectDisconnectPrint.Handle(inputDto, CurrentUser, false, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode, true);
            _jobClient.Enqueue(() => _smsHandler.Send("09925306265", result.ReportHeader.MessageText, Guid.NewGuid()));
            //result.ReportData.FirstOrDefault().MobileNumber

            return Ok(reportId);
        }

        [HttpPost]
        [Route("connect-set-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConnectSetResult([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            //await _customerUpdateHandler.Handle(input, _connectState, CurrentUser, cancellationToken);
            //use:IConnectDisconnectUpdateHandler
            return Ok(successfullyDone);
        }
        
        [HttpPost]
        [Route("disconnect-set-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisconnectSetResult([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            //await _customerUpdateHandler.Handle(input, _disconnectState, CurrentUser, cancellationToken);
            //use:IConnectDisconnectUpdateHandler
            return Ok(successfullyDone);
        }
    }
}
