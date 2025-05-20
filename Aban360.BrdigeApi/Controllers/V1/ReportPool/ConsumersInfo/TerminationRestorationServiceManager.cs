using Aban360.Common.Categories.ApiResponse;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/service-termination-restoration")]
    public class TerminationRestorationServiceManager : BaseController
    {
        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceTerminationRestorationDto>), StatusCodes.Status200OK)]
        public IActionResult GetInfo([FromBody] SearchInput searchInput)
        {
            ServiceTerminationRestorationDto info = new()
            {
                BillId = searchInput.Input,
                ServiceLinkDebt = 48000000,
                WaterBillDebt = 590000
            };
            return Ok(info);
        }

        [HttpPost]
        [Route("terminate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceTerminationRestorationDto>), StatusCodes.Status200OK)]
        public IActionResult TerminateServiceLink([FromBody] SearchInput searchInput)
        {           
            return Ok(searchInput);
        }

        [HttpPost]
        [Route("restore")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceTerminationRestorationDto>), StatusCodes.Status200OK)]
        public IActionResult RestoreServiceLink([FromBody] SearchInput searchInput)
        {
            return Ok(searchInput);
        }
    }
}
