using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1
{
    [Route("test")]
    [AllowAnonymous]
    public class TestController : BaseController
    {
        private readonly ISmsOldHandler _smsHandler;

        public TestController(ISmsOldHandler smsHandler)
        {
            _smsHandler = smsHandler;
            _smsHandler.NotNull(nameof(smsHandler));
        }

        [HttpPost]
        [Route("sms")]
        public IActionResult TesCalc()
        {
            BackgroundJob.Enqueue(() => _smsHandler.Send("09135742556", "این پیام جهت تست ارسال میگردد" + Environment.NewLine + "خط بعد"));
            return Ok();
        }
    }
    public class ZoneTest
    {
        //public ICollection<int>? ZoneIds { get; set; }
        public ICollection<int>? SelectedZoneIds { get; set; }
        public ICollection<int>? CategoryIds { get; set; }
    }
}
