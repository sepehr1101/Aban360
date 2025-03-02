using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering")]
    public class OfferingGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGetAllHandler _offeringGetAllHandler;
        public OfferingGetAllController(
            IUnitOfWork uow,
            IOfferingGetAllHandler offeringGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGetAllHandler = offeringGetAllHandler;
            _offeringGetAllHandler.NotNull(nameof(offeringGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var offerings = await _offeringGetAllHandler.Handle(cancellationToken);
            return Ok(offerings);
        }
    }
}
