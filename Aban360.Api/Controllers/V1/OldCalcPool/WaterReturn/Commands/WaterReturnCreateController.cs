using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Commands
{
    [Route("v1/water-return")]
    public class WaterReturnCreateController : BaseController
    {
        private readonly IRepairCreateHandler _repairHandler;
        public WaterReturnCreateController(IRepairCreateHandler repairHandler)
        {
            _repairHandler = repairHandler;
            _repairHandler.NotNull(nameof(repairHandler));
        }

        [HttpPost, HttpGet]
        [Route("create-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingToCreateRepairDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingToCreateRepairDto input, CancellationToken cancellationToken)
        {
            await _repairHandler.Handle(input, cancellationToken);
            return Ok(input);
        }

        [HttpPost, HttpGet]
        [Route("create-by-consumption")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateByConsumption([FromBody] MeterInfoWithMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _repairHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        [HttpPost, HttpGet]
        [Route("create-by-previous-consumption")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateByPreviousConsumption([FromBody] MeterInfoByLastMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _repairHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        [HttpPost, HttpGet]
        [Route("create-by-previous-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateByPreviousData([FromBody] MeterInfoByPreviousDataWithInvoiceIdInputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _repairHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
