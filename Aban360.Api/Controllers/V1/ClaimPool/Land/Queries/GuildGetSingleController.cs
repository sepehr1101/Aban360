using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/guild")]
    public class GuildGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildGetSingleHandler _guildHandler;
        public GuildGetSingleController(
            IUnitOfWork uow,
            IGuildGetSingleHandler guildHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var guild = await _guildHandler.Handle(id, cancellationToken);
            return Ok(guild);
        }
    }
}
