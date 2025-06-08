using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/unread")]
    public class UnreadController : BaseController
    {
        private readonly IUnreadHandler _unreadHandler;
        public UnreadController(IUnreadHandler unreadHandler)
        {
            _unreadHandler = unreadHandler;
            _unreadHandler.NotNull(nameof(unreadHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnreadInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto> unread = await _unreadHandler.Handle(input, cancellationToken);
            return Ok(unread);
        }
    }
}
