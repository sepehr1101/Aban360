using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-constant")]
    public class TariffConstantDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffConstantDeleteHandler _tariffConstantDeleteHandler;
        public TariffConstantDeleteController(
            IUnitOfWork uow,
            ITariffConstantDeleteHandler tariffConstantDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffConstantDeleteHandler = tariffConstantDeleteHandler;
            _tariffConstantDeleteHandler.NotNull(nameof(tariffConstantDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffConstantDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffConstantDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffConstantDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
