using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff")]
    public class TariffGetByLineItemTypeIdController : BaseController
    {
        private readonly ITariffGetByLineItemTypeIdHandler _tariffGetByLineItemTypeIdHandler;
        public TariffGetByLineItemTypeIdController(ITariffGetByLineItemTypeIdHandler tariffGetByLineItemTypeIdHandler)
        {
            _tariffGetByLineItemTypeIdHandler = tariffGetByLineItemTypeIdHandler;
            _tariffGetByLineItemTypeIdHandler.NotNull(nameof(tariffGetByLineItemTypeIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("by-line-item-type/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByOfferingId(short id, CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffGetByLineItemTypeIdHandler.Handle(id, cancellationToken);
            return Ok(tariffs);
        }
    }
}
