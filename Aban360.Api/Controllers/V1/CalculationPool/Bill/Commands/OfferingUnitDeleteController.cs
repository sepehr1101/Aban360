using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering-unit")]
    public class OfferingUnitDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUnitDeleteHandler _offeringUnitDeleteHandler;
        public OfferingUnitDeleteController(
            IUnitOfWork uow,
            IOfferingUnitDeleteHandler offeringUnitDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnitDeleteHandler = offeringUnitDeleteHandler;
            _offeringUnitDeleteHandler.NotNull(nameof(offeringUnitDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] OfferingUnitDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _offeringUnitDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
