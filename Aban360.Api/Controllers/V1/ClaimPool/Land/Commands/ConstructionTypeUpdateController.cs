﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/construction-type")]
    public class ConstructionTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConstructionTypeUpdateHandler _updateHandler;
        public ConstructionTypeUpdateController(
            IUnitOfWork uow,
            IConstructionTypeUpdateHandler updateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(_updateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] ConstructionTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _updateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
