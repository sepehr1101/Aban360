using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff")]
    public class TariffUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffUpdateHandler _tariffUpdateHandler;
        public TariffUpdateController(
            IUnitOfWork uow,
            ITariffUpdateHandler tariffUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffUpdateHandler = tariffUpdateHandler;
            _tariffUpdateHandler.NotNull(nameof(tariffUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
