using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCalculationModeUpdateHandler _tariffCalculationModeUpdateHandler;
        public TariffCalculationModeUpdateController(
            IUnitOfWork uow,
            ITariffCalculationModeUpdateHandler tariffCalculationModeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffCalculationModeUpdateHandler = tariffCalculationModeUpdateHandler;
            _tariffCalculationModeUpdateHandler.NotNull(nameof(tariffCalculationModeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffCalculationModeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
