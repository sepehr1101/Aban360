﻿using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-group")]
    public class OfferingGroupGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupGetSingleHandler _offeringGroupGetSingleHandler;
        public OfferingGroupGetSingleController(
            IUnitOfWork uow,
            IOfferingGroupGetSingleHandler offeringGroupGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGroupGetSingleHandler = offeringGroupGetSingleHandler;
            _offeringGroupGetSingleHandler.NotNull(nameof(offeringGroupGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var offeringGroups = await _offeringGroupGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offeringGroups);
        }
    }
}
