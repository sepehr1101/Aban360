using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/impact-sign")]
    public class ImpactSignCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IImpactSignCreateHandler _impactSignCreateHandler;
        public ImpactSignCreateController(
            IUnitOfWork uow,
            IImpactSignCreateHandler impactSignCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _impactSignCreateHandler = impactSignCreateHandler;
            _impactSignCreateHandler.NotNull(nameof(impactSignCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ImpactSignCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ImpactSignCreateDto createDto, CancellationToken cancellationToken)
        {
            await _impactSignCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
