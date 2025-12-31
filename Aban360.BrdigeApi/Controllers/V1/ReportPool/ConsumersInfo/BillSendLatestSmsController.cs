using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillSendLatestSmsController : BaseController
    {
        public BillSendLatestSmsController()
        {
        }

        [HttpPost]
        [Route("send-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(SearchInput input, CancellationToken cancellationToken)
        {
            //SendLatestSms
            return Ok(input);
        }
    }
}
