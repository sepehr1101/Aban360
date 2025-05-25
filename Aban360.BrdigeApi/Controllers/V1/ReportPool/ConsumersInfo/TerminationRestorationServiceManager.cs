using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/service-link")]
    public class ServiceLinkManagerController : BaseController
    {
        string successfullyDone = "با موفقیت انجام شد";
        [HttpPost]
        [Route("disconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public IActionResult Disconnect([FromBody] ServiceLinkConnectionInput input)
        {           
            return Ok(successfullyDone);
        }

        [HttpPost]
        [Route("reconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public IActionResult Reconnect([FromBody] ServiceLinkConnectionInput input)
        {            
            return Ok(successfullyDone);
        }
    }

    public record ServiceLinkConnectionInput
    {
        public string BillId { get; set; } = default!;
        public string? Description { get; set; }
        public string Who { get; set; } = default!;
        public DateTime When { get; set; }
        public string? How { get; set; }
        public string? Why { get; set; }
    }
}
