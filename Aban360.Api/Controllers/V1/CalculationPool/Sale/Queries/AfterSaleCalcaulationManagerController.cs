using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/after-sale")]
    public class AfterSaleCalcaulationManagerController : BaseController
    {
        private readonly IAfterSaleGetHandler _afterSaleGetHandler;
        public AfterSaleCalcaulationManagerController(IAfterSaleGetHandler afterSaleGetHandler)
        {
            _afterSaleGetHandler = afterSaleGetHandler;
            _afterSaleGetHandler.NotNull(nameof(afterSaleGetHandler));
        }

        [HttpPost]
        [Route("calculate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate([FromBody] AfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto> result = await _afterSaleGetHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
