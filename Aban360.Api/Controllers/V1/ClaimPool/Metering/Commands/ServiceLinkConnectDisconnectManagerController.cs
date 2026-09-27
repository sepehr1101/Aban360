using Aban360.Api.Cronjobs;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/service-link")]
    public class ServiceLinkConnectDisconnectManagerController : BaseController
    {
        private readonly IConnectDisconnectCommandHandler _connectDisconnectCommandHandler;
        private readonly IConnectDisconnectSetResultHandler _connectDisconnectSetResultHandler;
        private readonly IConnectDisconnectGetAllHandler _connectDisconnectGetAllHandler;
        private readonly IConnectDisconnectNoResultDeleteHandler _connectDisconnectNoResultDeleteHandler;
        private readonly IConnectDisconnectGetStiHandler _connectDisconnectGetStiHandler;
        private readonly IJudicialNoticeSetResultHandler _judicialNoticeSetResultHandler;
        private readonly IJudicialNoticeGetByIdHandler _judicialNoticeGetByIdHandler;
        private readonly IReportGenerator _reportGenerator;
        private readonly ISmsOldHandler _smsHandler;
        private readonly IBackgroundJobClient _jobClient;
        string successfullyDone = "با موفقیت انجام شد";
        public ServiceLinkConnectDisconnectManagerController(
            IConnectDisconnectCommandHandler connectDisconnectCommandHandler,
            IConnectDisconnectSetResultHandler connectDisconnectSetResultHandler,
            IConnectDisconnectGetAllHandler connectDisconnectGetAllHandler,
            IConnectDisconnectNoResultDeleteHandler connectDisconnectNoResultDeleteHandler,
            IConnectDisconnectGetStiHandler connectDisconnectGetStiHandler,
            IJudicialNoticeSetResultHandler judicialNoticeSetResultHandler,
            IJudicialNoticeGetByIdHandler judicialNoticeGetByIdHandler,
            IReportGenerator reportGenerator,
            ISmsOldHandler smsHandler,
            IBackgroundJobClient jobClient)
        {
            _connectDisconnectCommandHandler = connectDisconnectCommandHandler;
            _connectDisconnectCommandHandler.NotNull(nameof(connectDisconnectCommandHandler));

            _connectDisconnectSetResultHandler = connectDisconnectSetResultHandler;
            _connectDisconnectSetResultHandler.NotNull(nameof(connectDisconnectSetResultHandler));

            _connectDisconnectGetAllHandler = connectDisconnectGetAllHandler;
            _connectDisconnectGetAllHandler.NotNull(nameof(connectDisconnectGetAllHandler));

            _connectDisconnectNoResultDeleteHandler = connectDisconnectNoResultDeleteHandler;
            _connectDisconnectNoResultDeleteHandler.NotNull(nameof(connectDisconnectNoResultDeleteHandler));

            _connectDisconnectGetStiHandler = connectDisconnectGetStiHandler;
            _connectDisconnectGetStiHandler.NotNull(nameof(connectDisconnectGetStiHandler));

            _judicialNoticeSetResultHandler = judicialNoticeSetResultHandler;
            _judicialNoticeSetResultHandler.NotNull(nameof(judicialNoticeSetResultHandler));

            _judicialNoticeGetByIdHandler = judicialNoticeGetByIdHandler;
            _judicialNoticeGetByIdHandler.NotNull(nameof(judicialNoticeGetByIdHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));

            _smsHandler = smsHandler;
            _smsHandler.NotNull(nameof(smsHandler));

            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));
        }


        [HttpPost]
        [Route("connect-command")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConnectCommandSti(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2070;
            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await _connectDisconnectCommandHandler.Handle(inputDto, CurrentUser, true, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode, true);

            string mobileNumber = result.ReportData?.FirstOrDefault()?.MobileNumber ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(mobileNumber))
                _jobClient.Enqueue(() => _smsHandler.Send(mobileNumber, result.ReportHeader.MessageText, Guid.NewGuid()));

            return Ok(reportId);
        }

        [HttpPost]
        [Route("disconnect-command")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDisconnectCommandSti(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2071;
            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await _connectDisconnectCommandHandler.Handle(inputDto, CurrentUser, false, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode, true);

            string mobileNumber = result.ReportData?.FirstOrDefault()?.MobileNumber ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(mobileNumber))
                _jobClient.Enqueue(() => _smsHandler.Send(mobileNumber, result.ReportHeader.MessageText, Guid.NewGuid()));

            return Ok(reportId);
        }

        [HttpPost]
        [Route("connect-set-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConnectSetResult([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            ConnectDisconnectSetResultOutputDto result = await _connectDisconnectSetResultHandler.Handle(input, true, CurrentUser, cancellationToken);
            if (result.IsSend && !string.IsNullOrWhiteSpace(result.MobileNumber))
                _jobClient.Enqueue(() => _smsHandler.Send(result.MobileNumber, result.Message, Guid.NewGuid()));

            return Ok(successfullyDone);
        }

        [HttpPost]
        [Route("disconnect-set-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisconnectSetResult([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            ConnectDisconnectSetResultOutputDto result = await _connectDisconnectSetResultHandler.Handle(input, false, CurrentUser, cancellationToken);
            if (result.IsSend && !string.IsNullOrWhiteSpace(result.MobileNumber))
                _jobClient.Enqueue(() => _smsHandler.Send(result.MobileNumber, result.Message, Guid.NewGuid()));

            return Ok(successfullyDone);
        }

        [HttpGet]
        [Route("disconnect-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ServiceLinkDisconnectResultDto>>), StatusCodes.Status200OK)]
        public IActionResult GetDisconnectResultDictionary(CancellationToken cancellationToken)
        {
            ICollection<ServiceLinkDisconnectResultDto> dictionary = _connectDisconnectSetResultHandler.GetDisconnectResults();
            return Ok(dictionary);
        }

        [HttpGet]
        [Route("connect-disconnect-result/{typeId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ServiceLinkDisconnectResultDto>>), StatusCodes.Status200OK)]
        public IActionResult GetDisconnectResultDictionary(int typeId, CancellationToken cancellationToken)
        {
            ICollection<ServiceLinkDisconnectResultDto> result = new List<ServiceLinkDisconnectResultDto>();

            if (typeId == ReportLiterals.DisconnectId)
            {
                result = _connectDisconnectSetResultHandler.GetDisconnectResults();
            }
            if (typeId == ReportLiterals.JudicalNoticeId)
            {
                result = _judicialNoticeSetResultHandler.GetJudicalNoticeResults();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("connect-disconnect-sub-result/{typeId}/{subTypeId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetDisconnectResultDictionary(int typeId, int subTypeId, CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> result = new List<NumericDictionary>();

            if (typeId == ReportLiterals.JudicalNoticeId && subTypeId == 2)
            {
                result = _judicialNoticeSetResultHandler.GetCustomerDebtSubResult();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("unconfirmed/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConnectDisconnectHeaderOutputDto, ConnectDisconnectDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnconfirmed(int zoneId, CancellationToken cancellationToken)
        {
            ReportOutput<ConnectDisconnectHeaderOutputDto, ConnectDisconnectDataOutputDto> result = await _connectDisconnectGetAllHandler.Handle(zoneId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConnectDisconnectRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] ConnectDisconnectRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _connectDisconnectNoResultDeleteHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("sti/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSti(long id, CancellationToken cancellationToken)
        {
            int connectReportCode = (int)StiReportCodeLiterals.ConnectServiceLink;
            int disconnectReportCode = (int)StiReportCodeLiterals.DisconnectServiceLink;
            int judicialNoticeReportCode = (int)StiReportCodeLiterals.JudicialNoticeServiceLink;
            int reportCode = 0;
            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> connectDisconnectResult = await _connectDisconnectGetStiHandler.Handle(id, CurrentUser, cancellationToken);

            int typeCode = connectDisconnectResult.ReportData.FirstOrDefault().TypeId;
            if (typeCode == ReportLiterals.ConnectId)
            {
                JsonReportId reportId = await JsonOperation.ExportToJsonFlat(connectDisconnectResult, cancellationToken, connectReportCode, true);
                return Ok(reportId);

            }
            else if (typeCode == ReportLiterals.DisconnectId)
            {
                JsonReportId reportId = await JsonOperation.ExportToJsonFlat(connectDisconnectResult, cancellationToken, disconnectReportCode, true);
                return Ok(reportId);
            }
            else if (typeCode == ReportLiterals.JudicalNoticeId)
            {
                FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto> judicialResult = await _judicialNoticeGetByIdHandler.Handle(id, CurrentUser, cancellationToken);
                JsonReportId reportId = await JsonOperation.ExportToJsonFlat(judicialResult, cancellationToken, judicialNoticeReportCode, true);
                return Ok(reportId);
            }
            return Ok(id);
        }
    }
}
