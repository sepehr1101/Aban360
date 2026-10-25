using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.NotificationPool.Domain.Features.Sms;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Api
{
    [Route("v1/sms")]
    public class SendSmsTestController : BaseController
    {
        private readonly ISmsOldHandler _sendSms;
        public SendSmsTestController(ISmsOldHandler sendSms)
        {
            _sendSms = sendSms;
            _sendSms.NotNull(nameof(sendSms));
        }

        [HttpPost]
        [Route("test-batch")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SendSmsDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Test4040(CancellationToken cancellationToken)
        {
            ICollection<SendSmsDataDto> data = new List<SendSmsDataDto>()
            {
                new SendSmsDataDto(":( پیام تست ابفا","09925306265", "1130432816316","1020300",Guid.Empty  ),
                new SendSmsDataDto(":) پیام تست ابفا","09135742556", "10018315","4050600",Guid.Empty  ),
                new SendSmsDataDto("^_^ پیام تست ابفا","09391294911","1100200010","7080900",Guid.Empty  ),

            };
            await _sendSms.Send(data);
            return Ok(data);
        }
    }
}
