using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff")]
    public class TariffDuplicateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffDuplicateHandler _tariffDuplicateHandler;
        public TariffDuplicateController(
            IUnitOfWork uow,
            ITariffDuplicateHandler tariffDuplicateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffDuplicateHandler = tariffDuplicateHandler;
            _tariffDuplicateHandler.NotNull(nameof(tariffDuplicateHandler));
        }

        [HttpPost]
        [Route("duplicate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffDuplicateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Duplicate([FromBody] TariffDuplicateDto DuplicateDto, CancellationToken cancellationToken)
        {
            await _tariffDuplicateHandler.Handle(DuplicateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(DuplicateDto);
        }
    }
}
