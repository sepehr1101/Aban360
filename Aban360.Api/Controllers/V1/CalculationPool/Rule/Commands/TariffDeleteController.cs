using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff")]
    public class TariffDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffDeleteHandler _tariffDeleteHandler;
        public TariffDeleteController(
            IUnitOfWork uow,
            ITariffDeleteHandler tariffDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffDeleteHandler = tariffDeleteHandler;
            _tariffDeleteHandler.NotNull(nameof(tariffDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
