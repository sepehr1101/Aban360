using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-by-detail")]
    public class TariffByDetailCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCalculationByDetailHandler _tariffByDetailCreateHandler;
        public TariffByDetailCreateController(
            IUnitOfWork uow,
            ITariffCalculationByDetailHandler tariffByDetailCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffByDetailCreateHandler = tariffByDetailCreateHandler;
            _tariffByDetailCreateHandler.NotNull(nameof(tariffByDetailCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffByDetailCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffByDetailCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffByDetailCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
