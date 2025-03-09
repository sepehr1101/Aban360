using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-constant")]
    public class TariffConstantCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffConstantCreateHandler _tariffConstantCreateHandler;
        public TariffConstantCreateController(
            IUnitOfWork uow,
            ITariffConstantCreateHandler tariffConstantCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffConstantCreateHandler = tariffConstantCreateHandler;
            _tariffConstantCreateHandler.NotNull(nameof(tariffConstantCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffConstantCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffConstantCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffConstantCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
