using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Aban360.Api.Controllers.V1
{
    [Route("test")]
    [AllowAnonymous]
    public class TestController : BaseController
    {
        private readonly ISmsOldHandler _smsHandler;
        IHttpContextAccessor _contextAccessor;

        public TestController(ISmsOldHandler smsHandler, IHttpContextAccessor httpContextAccessor)
        {
            _smsHandler = smsHandler;
            _smsHandler.NotNull(nameof(smsHandler));

            _contextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("sms")]
        public IActionResult TesCalc()
        {
            BackgroundJob.Enqueue(() => _smsHandler.Send("09135742556", "این پیام جهت تست ارسال میگردد" + Environment.NewLine + "خط بعد",Guid.NewGuid()));
            return Ok();
        }

        [HttpPost]
        [Route("check-body")]
        public async Task<IActionResult> TestBody()
        {
            var requestBody = await new StreamReader(_contextAccessor.HttpContext.Request.Body)
                 .ReadToEndAsync();
            AssessmentResultInputDto input = JsonOperation.Unmarshal<AssessmentResultInputDto>(requestBody);

            return Ok(requestBody);
        }
    }
    public class ZoneTest
    {
        //public ICollection<int>? ZoneIds { get; set; }
        public ICollection<int>? SelectedZoneIds { get; set; }
        public ICollection<int>? CategoryIds { get; set; }
    }
}
