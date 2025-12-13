using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class LatestReadingBillController : BaseController
    {
        private readonly ILatestReadingBillHandler _latestReadingBillHandler;
        public LatestReadingBillController(
            ILatestReadingBillHandler latestReadingBillHandler)
        {
            _latestReadingBillHandler = latestReadingBillHandler;
            _latestReadingBillHandler.NotNull(nameof(latestReadingBillHandler));
        }

        [HttpPost]
        [Route("latest/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestReadingBillDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(string billId, CancellationToken cancellationToken)
        {
            LatestReadingBillDataOutputDto result = await _latestReadingBillHandler.Handle(billId, cancellationToken);
            return Ok(result);
        }
    }
}
