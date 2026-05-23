using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/guild")]
    public class GuildGetSingleController : BaseController
    {
        private readonly IGuildGetSingleHandler _guildHandler;
        private readonly IGuildByIdQueryHandler _guildByIdQueryHandler;
        public GuildGetSingleController(
            IGuildGetSingleHandler guildHandler,
            IGuildByIdQueryHandler guildByIdQueryHandler)
        {
            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));

            _guildByIdQueryHandler = guildByIdQueryHandler;
            _guildByIdQueryHandler.NotNull(nameof(guildByIdQueryHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            //GuildGetDto guild = await _guildHandler.Handle(id, cancellationToken);
            NumericDictionary result = await _guildByIdQueryHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
