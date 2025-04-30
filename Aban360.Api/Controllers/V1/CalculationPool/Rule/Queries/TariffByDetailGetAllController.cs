using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-by-detail")]
    public class TariffByDetailGetAllController : BaseController
    {
        private readonly ITariffByDetailGetAllHandler _tariffByDetailGetAllHandler;
        public TariffByDetailGetAllController(ITariffByDetailGetAllHandler tariffByDetailGetAllHandler)
        {
            _tariffByDetailGetAllHandler = tariffByDetailGetAllHandler;
            _tariffByDetailGetAllHandler.NotNull(nameof(tariffByDetailGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffByDetailGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tariffByDetails = await _tariffByDetailGetAllHandler.Handle(cancellationToken);
            return Ok(tariffByDetails);
        }
    }
}
