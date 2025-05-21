using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerMobileInfoManager : BaseController
    {
        [HttpPost]
        [Route("mobile-numbers")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillIdMobileDto>), StatusCodes.Status200OK)]
        public IActionResult GetMobiles([FromBody] BillIdListDtoWrapper billIdListDtoWrapper, CancellationToken cancellationToken)
        {
            var list = new List<BillIdMobileDto>
            {
                new BillIdMobileDto {BillId="123456",Mobile="09130000000" },
                new BillIdMobileDto {BillId="9876543",Mobile="09131111111" },
            };
            return Ok(list);
        }
    }
    public record BillIdMobileDto
    {
        public string BillId { get; set; } = default!;
        public string Mobile { get; set; } = default!;
    }
    public record BillIdListDto
    {
        public string BillId { get; set; } = default!;
    }
    public record BillIdListDtoWrapper
    {
        ICollection<BillIdListDto>? BillIdList { get; set; }
    }
}
