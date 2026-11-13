using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    public class ReadingController : BaseController
    {
        private readonly ILatestReadingBillHandler _c0_latestReadingBillHandler;
        private readonly IReadingInBetweenHandler _c1_handler;
        public ReadingController(ILatestReadingBillHandler c0_latestReadingBillHandler, IReadingInBetweenHandler c1_handler)
        {
            _c0_latestReadingBillHandler = c0_latestReadingBillHandler;
            _c0_latestReadingBillHandler.NotNull(nameof(c0_latestReadingBillHandler));
            _c1_handler = c1_handler;
            _c1_handler.NotNull(nameof(_c1_handler));
        }

        [HttpPost]
        [Route("~/v1/reading/latest")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestReadingBillDataOutputDto>), StatusCodes.Status200OK)]
        [ActionName("GetRaw")]
        public async Task<IActionResult> GetRawFromLatestReadingBill0([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            LatestReadingBillDataOutputDto result = await _c0_latestReadingBillHandler.Handle_WithPreviousDb(input.Input, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/reading/in-between")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestReadingBillDataOutputDto>), StatusCodes.Status200OK)]
        [ActionName("GetRaw")]
        public async Task<IActionResult> GetRawFromReadingHelper1([FromBody] ReadingInBetweenInput input, CancellationToken cancellationToken)
        {
            IEnumerable<ReadingInBetweenOutput> result = await _c1_handler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
