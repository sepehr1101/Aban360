using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering-group")]
    public class OfferingGroupDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupDeleteHandler _offeringGroupDeleteHandler;
        public OfferingGroupDeleteController(
            IUnitOfWork uow,
            IOfferingGroupDeleteHandler offeringGroupDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGroupDeleteHandler = offeringGroupDeleteHandler;
            _offeringGroupDeleteHandler.NotNull(nameof(offeringGroupDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] OfferingGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _offeringGroupDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
