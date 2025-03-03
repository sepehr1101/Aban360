using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering")]
    public class OfferingDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingDeleteHandler _offeringDeleteHandler;
        public OfferingDeleteController(
            IUnitOfWork uow,
            IOfferingDeleteHandler offeringDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringDeleteHandler = offeringDeleteHandler;
            _offeringDeleteHandler.NotNull(nameof(offeringDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfferingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _offeringDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
