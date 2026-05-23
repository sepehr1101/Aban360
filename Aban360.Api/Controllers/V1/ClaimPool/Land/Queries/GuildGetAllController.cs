using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/guild")]
    public class GuildGetAllController : BaseController
    {
        private readonly IGuildGetAllHandler _guildHandler;
        private readonly IGuildQueryHandler _guildQueryHandler;
        public GuildGetAllController(
            IGuildGetAllHandler guildHandler,
            IGuildQueryHandler guildQueryHandler)
        {
            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));

            _guildQueryHandler = guildQueryHandler;
            _guildQueryHandler.NotNull(nameof(guildQueryHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            //ICollection<GuildGetDto> guild = await _guildHandler.Handle(cancellationToken);
            IEnumerable<NumericDictionary> guild = await _guildQueryHandler.Handle(cancellationToken);
            return Ok(guild);
        }
    }
}
