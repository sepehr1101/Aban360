﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/construction-type")]
    public class ConstructionTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConstructionTypeGetSingleHandler _getSingleHandler;
        public ConstructionTypeGetSingleController(
            IUnitOfWork uow,
            IConstructionTypeGetSingleHandler getSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getSingleHandler = getSingleHandler;
            _getSingleHandler.NotNull(nameof(_getSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConstructionTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ConstructionTypeGetDto result = await _getSingleHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
