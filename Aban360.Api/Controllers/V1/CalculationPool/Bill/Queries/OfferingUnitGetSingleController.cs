using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-unit")]
    public class OfferingUnitGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUnitGetSingleHandler _offeringUnitGetSingleHandler;
        public OfferingUnitGetSingleController(
            IUnitOfWork uow,
            IOfferingUnitGetSingleHandler offeringUnitGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnitGetSingleHandler = offeringUnitGetSingleHandler;
            _offeringUnitGetSingleHandler.NotNull(nameof(offeringUnitGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var offeringUnits = await _offeringUnitGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offeringUnits);
        }
    }
}
