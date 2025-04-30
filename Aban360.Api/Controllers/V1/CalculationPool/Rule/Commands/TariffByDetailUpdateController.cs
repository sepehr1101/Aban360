using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-by-detail")]
    public class TariffByDetailUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffByDetailUpdateHandler _tariffByDetailUpdateHandler;
        public TariffByDetailUpdateController(
            IUnitOfWork uow,
            ITariffByDetailUpdateHandler tariffByDetailUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffByDetailUpdateHandler = tariffByDetailUpdateHandler;
            _tariffByDetailUpdateHandler.NotNull(nameof(tariffByDetailUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffByDetailUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffByDetailUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffByDetailUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
