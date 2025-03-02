using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering-group")]
    public class OfferingGroupCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupCreateHandler _offeringGroupCreateHandler;
        public OfferingGroupCreateController(
            IUnitOfWork uow,
            IOfferingGroupCreateHandler offeringGroupCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGroupCreateHandler = offeringGroupCreateHandler;
            _offeringGroupCreateHandler.NotNull(nameof(offeringGroupCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] OfferingGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            await _offeringGroupCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
