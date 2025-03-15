using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/impact-sign")]
    public class ImpactSignUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IImpactSignUpdateHandler _impactSignUpdateHandler;
        public ImpactSignUpdateController(
            IUnitOfWork uow,
            IImpactSignUpdateHandler impactSignUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _impactSignUpdateHandler = impactSignUpdateHandler;
            _impactSignUpdateHandler.NotNull(nameof(impactSignUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ImpactSignUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ImpactSignUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _impactSignUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
