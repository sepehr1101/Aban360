using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/guild")]
    public class GuildCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildCreateHandler _guildHandler;
        public GuildCreateController(
            IUnitOfWork uow,
            IGuildCreateHandler GuildHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guildHandler = GuildHandler;
            _guildHandler.NotNull(nameof(GuildHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] GuildCreateDto createDto, CancellationToken cancellationToken)
        {
            await _guildHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
