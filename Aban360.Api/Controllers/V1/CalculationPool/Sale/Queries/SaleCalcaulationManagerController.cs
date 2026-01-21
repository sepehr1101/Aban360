using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/sale")]
    public class SaleCalcaulationManagerController : BaseController
    {
        private readonly ISaleGetHandler _getHandler;
        public SaleCalcaulationManagerController(ISaleGetHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpPost]
        [Route("calculate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate([FromBody] SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> result = await _getHandler.Handle(inputDto, cancellationToken);

            return Ok(result);
        }
    }
}
