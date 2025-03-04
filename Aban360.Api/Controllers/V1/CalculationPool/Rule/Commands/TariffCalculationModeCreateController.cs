using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCalculationModeCreateHandler _tariffCalculationModeCreateHandler;
        public TariffCalculationModeCreateController(
            IUnitOfWork uow,
            ITariffCalculationModeCreateHandler tariffCalculationModeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffCalculationModeCreateHandler = tariffCalculationModeCreateHandler;
            _tariffCalculationModeCreateHandler.NotNull(nameof(tariffCalculationModeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffCalculationModeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
