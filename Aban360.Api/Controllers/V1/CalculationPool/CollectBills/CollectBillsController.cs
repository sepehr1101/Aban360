using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.CollectBills
{
    [Route("v1/collect-bills")]
    public class CollectBillsController : BaseController
    {
        private readonly ICollectBillsDetailJobService _jobService;
        public CollectBillsController(ICollectBillsDetailJobService jobService)
        {
            _jobService = jobService;
            _jobService.NotNull(nameof(jobService));
        }

        [HttpPost, HttpGet]
        [Route("send-file")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CollectBillsDetailGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendFile(CancellationToken cancellationToken)
        {
            await _jobService.Initialize();
            return Ok();
        }
    }
}
