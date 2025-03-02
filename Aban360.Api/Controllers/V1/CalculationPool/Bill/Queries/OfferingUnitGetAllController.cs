using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-unit")]
    public class OfferingUnitGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUnitGetAllHandler _offeringUnitGetAllHandler;
        public OfferingUnitGetAllController(
            IUnitOfWork uow,
            IOfferingUnitGetAllHandler offeringUnitGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnitGetAllHandler = offeringUnitGetAllHandler;
            _offeringUnitGetAllHandler.NotNull(nameof(offeringUnitGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var offeringUnits = await _offeringUnitGetAllHandler.Handle(cancellationToken);
            return Ok(offeringUnits);
        }
    }
}
