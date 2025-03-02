using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering-group")]
    public class OfferingGroupGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupGetAllHandler _offeringGroupGetAllHandler;
        public OfferingGroupGetAllController(
            IUnitOfWork uow,
            IOfferingGroupGetAllHandler offeringGroupGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGroupGetAllHandler = offeringGroupGetAllHandler;
            _offeringGroupGetAllHandler.NotNull(nameof(offeringGroupGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var offeringGroups = await _offeringGroupGetAllHandler.Handle(cancellationToken);
            return Ok(offeringGroups);
        }
    }
}
