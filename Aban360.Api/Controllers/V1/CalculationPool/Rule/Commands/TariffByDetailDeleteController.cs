using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-by-detail")]
    public class TariffByDetailDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffByDetailDeleteHandler _tariffByDetailDeleteHandler;
        public TariffByDetailDeleteController(
            IUnitOfWork uow,
            ITariffByDetailDeleteHandler tariffByDetailDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffByDetailDeleteHandler = tariffByDetailDeleteHandler;
            _tariffByDetailDeleteHandler.NotNull(nameof(tariffByDetailDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffByDetailDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffByDetailDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffByDetailDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
