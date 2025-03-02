using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/offering")]
    public class OfferingGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGetSingleHandler _offeringGetSingleHandler;
        public OfferingGetSingleController(
            IUnitOfWork uow,
            IOfferingGetSingleHandler offeringGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGetSingleHandler = offeringGetSingleHandler;
            _offeringGetSingleHandler.NotNull(nameof(offeringGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var offerings = await _offeringGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offerings);
        }
    }
}
