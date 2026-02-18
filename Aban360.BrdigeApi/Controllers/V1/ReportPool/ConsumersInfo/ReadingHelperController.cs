using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/reading")]
    public class ReadingHelperController : BaseController
    {
        private readonly IReadingInBetweenHandler _handler;
        public ReadingHelperController(
            IReadingInBetweenHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(_handler));
        }

        [HttpPost]
        [Route("in-between")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestReadingBillDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] ReadingInBetweenInput input, CancellationToken cancellationToken)
        {
            IEnumerable<ReadingInBetweenOutput> result = await _handler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
