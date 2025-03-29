﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/user-leave")]
    public class UserLeaveGetSingleController : BaseController
    {
        private readonly IUserLeaveGetSingleHandler _userLeaveGetSingleHandler;
        public UserLeaveGetSingleController(IUserLeaveGetSingleHandler UserLeaveGetSingleHandler)
        {
            _userLeaveGetSingleHandler = UserLeaveGetSingleHandler;
            _userLeaveGetSingleHandler.NotNull(nameof(UserLeaveGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserLeaveGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var userLeaves = await _userLeaveGetSingleHandler.Handle(id, cancellationToken);
            return Ok(userLeaves);
        }
    }
}
