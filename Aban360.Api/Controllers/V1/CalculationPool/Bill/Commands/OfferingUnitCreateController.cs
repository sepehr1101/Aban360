using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering-unit")]
    public class OfferingUnitCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUnitCreateHandler _offeringUnitCreateHandler;
        public OfferingUnitCreateController(
            IUnitOfWork uow,
            IOfferingUnitCreateHandler offeringUnitCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnitCreateHandler = offeringUnitCreateHandler;
            _offeringUnitCreateHandler.NotNull(nameof(offeringUnitCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] OfferingUnitCreateDto createDto, CancellationToken cancellationToken)
        {
            await _offeringUnitCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
