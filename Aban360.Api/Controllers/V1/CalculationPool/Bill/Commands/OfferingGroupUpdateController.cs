﻿using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering-group")]
    public class OfferingGroupUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupUpdateHandler _offeringGroupUpdateHandler;
        public OfferingGroupUpdateController(
            IUnitOfWork uow,
            IOfferingGroupUpdateHandler offeringGroupUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGroupUpdateHandler = offeringGroupUpdateHandler;
            _offeringGroupUpdateHandler.NotNull(nameof(offeringGroupUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _offeringGroupUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
