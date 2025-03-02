using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering")]
    public class OfferingUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUpdateHandler _offeringUpdateHandler;
        public OfferingUpdateController(
            IUnitOfWork uow,
            IOfferingUpdateHandler offeringUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUpdateHandler = offeringUpdateHandler;
            _offeringUpdateHandler.NotNull(nameof(offeringUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] OfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _offeringUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
