using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.NotificationPool.Domain.Features.Sms;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1
{
    public class SmsController : BaseController
    {
        private readonly ISmsOldHandler _c0_sendSms;
        private readonly IReceivedSmdByPaigingHandler _c1_receivedSmdByPaigingHandler;
        public SmsController(ISmsOldHandler c0_sendSms, IReceivedSmdByPaigingHandler c1_receivedSmdByPaigingHandler)
        {
            _c0_sendSms = c0_sendSms;
            _c0_sendSms.NotNull(nameof(c0_sendSms));
            _c1_receivedSmdByPaigingHandler = c1_receivedSmdByPaigingHandler;
            _c1_receivedSmdByPaigingHandler.NotNull(nameof(c1_receivedSmdByPaigingHandler));
        }

        [HttpPost]
        [Route("~/v1/sms/test-batch")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SendSmsDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Test4040(CancellationToken cancellationToken)
        {
            ICollection<SendSmsDataDto> data = new List<SendSmsDataDto>()
            {
                new SendSmsDataDto(":( پیام تست ابفا", "09925306265", "1130432816316", "1020300", Guid.Empty),
                new SendSmsDataDto(":) پیام تست ابفا", "09135742556", "10018315", "4050600", Guid.Empty),
                new SendSmsDataDto("^_^ پیام تست ابفا", "09391294911", "1100200010", "7080900", Guid.Empty),
            };
            await _c0_sendSms.Send(data);
            return Ok(data);
        }

        [HttpPost]
        [Route("~/v1/sms/received-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivedSmsByPaiginationRaw([FromBody] ReceivedSmsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto> result = await _c1_receivedSmdByPaigingHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/sms/received-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivedSmsByPaiginationSti([FromBody] ReceivedSmsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2060;
            ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto> result = await _c1_receivedSmdByPaigingHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
