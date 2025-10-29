using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/sale")]
    public class SaleGetController : BaseController
    {
        private readonly ISaleGetHandler _getHandler;
        public SaleGetController(ISaleGetHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> result = await _getHandler.Handle(inputDto, cancellationToken);

            return Ok(result);
        }
    }
}
