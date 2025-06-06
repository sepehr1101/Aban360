﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/guild")]
    public class GuildGetSingleController : BaseController
    {       
        private readonly IGuildGetSingleHandler _guildHandler;
        public GuildGetSingleController(
            IGuildGetSingleHandler guildHandler)
        {
            _guildHandler = guildHandler;
            _guildHandler.NotNull(nameof(guildHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            GuildGetDto guild = await _guildHandler.Handle(id, cancellationToken);
            return Ok(guild);
        }
    }
}
