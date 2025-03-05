using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-constant")]
    public class TariffConstantGetAllController : BaseController
    {
        private readonly ITariffConstantGetAllHandler _tariffConstantGetAllHandler;
        public TariffConstantGetAllController(ITariffConstantGetAllHandler tariffConstantGetAllHandler)
        {
            _tariffConstantGetAllHandler = tariffConstantGetAllHandler;
            _tariffConstantGetAllHandler.NotNull(nameof(tariffConstantGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffConstantGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tariffConstants = await _tariffConstantGetAllHandler.Handle(cancellationToken);
            return Ok(tariffConstants);
        }
    }
}
