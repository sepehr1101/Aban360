using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/offering")]
    public class OfferingCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingCreateHandler _offeringCreateHandler;
        public OfferingCreateController(
            IUnitOfWork uow,
            IOfferingCreateHandler offeringCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringCreateHandler = offeringCreateHandler;
            _offeringCreateHandler.NotNull(nameof(offeringCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _offeringCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
