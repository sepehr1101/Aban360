using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering-unit")]
    public class OfferingUnitUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUnitUpdateHandler _offeringUnitUpdateHandler;
        public OfferingUnitUpdateController(
            IUnitOfWork uow,
            IOfferingUnitUpdateHandler offeringUnitUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnitUpdateHandler = offeringUnitUpdateHandler;
            _offeringUnitUpdateHandler.NotNull(nameof(offeringUnitUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] OfferingUnitUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _offeringUnitUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
