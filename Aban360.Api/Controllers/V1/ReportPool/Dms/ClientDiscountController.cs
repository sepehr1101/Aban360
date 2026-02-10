using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Dms;
using Aban360.ReportPool.Persistence.Features.Dms.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Dms
{
    [Route("v1/client-discount")]
    public class ClientDiscountController : BaseController
    {
        private readonly IRequestDiscountService _requestDiscountService;
        public ClientDiscountController(IRequestDiscountService requestDiscountService)
        {
            _requestDiscountService=requestDiscountService;
            _requestDiscountService.NotNull(nameof(requestDiscountService));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ClientDiscount>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var data = await _requestDiscountService.Get();
            return Ok(data);
        }
    }
}
