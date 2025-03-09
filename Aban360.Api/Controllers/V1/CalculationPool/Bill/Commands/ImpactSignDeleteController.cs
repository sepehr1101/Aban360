using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/impact-sign")]
    public class ImpactSignDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IImpactSignDeleteHandler _impactSignDeleteHandler;
        public ImpactSignDeleteController(
            IUnitOfWork uow,
            IImpactSignDeleteHandler impactSignDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _impactSignDeleteHandler = impactSignDeleteHandler;
            _impactSignDeleteHandler.NotNull(nameof(impactSignDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ImpactSignDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ImpactSignDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _impactSignDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
