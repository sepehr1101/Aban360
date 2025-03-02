using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/guild")]
    public class GuildUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildUpdateHandler _guildHandler;
        public GuildUpdateController(
            IUnitOfWork uow,
            IGuildUpdateHandler guildHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] GuildUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _guildHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
