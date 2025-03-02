using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/guild")]
    public class GuildGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildGetAllHandler _guildHandler;
        public GuildGetAllController(
            IUnitOfWork uow,
            IGuildGetAllHandler guildHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var guild = await _guildHandler.Handle(cancellationToken);
            return Ok(guild);
        }
    }
}
