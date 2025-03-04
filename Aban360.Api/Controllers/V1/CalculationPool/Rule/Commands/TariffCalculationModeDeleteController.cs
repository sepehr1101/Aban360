using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCalculationModeDeleteHandler _tariffCalculationModeDeleteHandler;
        public TariffCalculationModeDeleteController(
            IUnitOfWork uow,
            ITariffCalculationModeDeleteHandler tariffCalculationModeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffCalculationModeDeleteHandler = tariffCalculationModeDeleteHandler;
            _tariffCalculationModeDeleteHandler.NotNull(nameof(tariffCalculationModeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffCalculationModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffCalculationModeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
