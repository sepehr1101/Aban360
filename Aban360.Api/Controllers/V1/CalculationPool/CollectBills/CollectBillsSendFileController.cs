using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.CollectBills
{
    [Route("v1/collect-bills")]
    public class CollectBillsSendFileController : BaseController
    {
        private readonly ICollectBillsDetailJobService _jobService;
        public CollectBillsSendFileController(ICollectBillsDetailJobService jobService)
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
            //await _jobService.Upload(Guid.Parse("0BEFA11B-0924-4238-B151-AFB7FB7C98BD"), "14050602-15-01-18-CollectBills.zip");
            return Ok();
        }
    }
}
