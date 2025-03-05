using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff")]
    public class TariffCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCreateHandler _tariffCreateHandler;
        public TariffCreateController(
            IUnitOfWork uow,
            ITariffCreateHandler tariffCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffCreateHandler = tariffCreateHandler;
            _tariffCreateHandler.NotNull(nameof(tariffCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
