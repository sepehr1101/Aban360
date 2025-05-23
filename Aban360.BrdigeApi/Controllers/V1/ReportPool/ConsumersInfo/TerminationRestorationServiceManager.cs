using Aban360.Common.Categories.ApiResponse;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
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
        public IActionResult Disconnect([FromBody] SearchInput searchInput)
        {           
            return Ok(successfullyDone);
        }

        [HttpPost]
        [Route("reconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public IActionResult Reconnect([FromBody] SearchInput searchInput)
        {            
            return Ok(successfullyDone);
        }
    }
}
