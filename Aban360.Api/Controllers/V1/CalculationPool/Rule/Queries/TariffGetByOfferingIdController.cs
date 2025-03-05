using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff")]
    public class TariffGetByOfferingIdController : BaseController
    {
        private readonly ITariffGetByOfferingIdHandler _tariffGetByOfferingIdHandler;
        public TariffGetByOfferingIdController(ITariffGetByOfferingIdHandler tariffGetByOfferingIdHandler)
        {
            _tariffGetByOfferingIdHandler = tariffGetByOfferingIdHandler;
            _tariffGetByOfferingIdHandler.NotNull(nameof(tariffGetByOfferingIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("by-offering/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByOfferingId(short id, CancellationToken cancellationToken)
        {
            var tariffs = await _tariffGetByOfferingIdHandler.Handle(id, cancellationToken);
            return Ok(tariffs);
        }
    }
}
