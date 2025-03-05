using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff")]
    public class TariffGetAllController : BaseController
    {
        private readonly ITariffGetAllHandler _tariffGetAllHandler;
        public TariffGetAllController(ITariffGetAllHandler tariffGetAllHandler)
        {
            _tariffGetAllHandler = tariffGetAllHandler;
            _tariffGetAllHandler.NotNull(nameof(tariffGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tariffs = await _tariffGetAllHandler.Handle(cancellationToken);
            return Ok(tariffs);
        }
    }
}
