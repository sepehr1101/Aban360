using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/guild")]
    public class GuildDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildDeleteHandler _guildHandler;
        public GuildDeleteController(
            IUnitOfWork uow,
            IGuildDeleteHandler guildHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GuildDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _guildHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
