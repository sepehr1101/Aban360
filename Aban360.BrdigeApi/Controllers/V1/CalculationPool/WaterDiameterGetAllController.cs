using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using ISaleDictionariesService = global::Aban360.CalculationPool.Application.Features.Sale.Services.ISaleDictionariesService;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/water-diameter")]
    public class WaterDiameterGetAllController : BaseController
    {
        private readonly ISaleDictionariesService _saleDictionariesService;
        public WaterDiameterGetAllController(ISaleDictionariesService saleDictionariesService)
        {
            _saleDictionariesService = saleDictionariesService;
            _saleDictionariesService.NotNull(nameof(saleDictionariesService));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _saleDictionariesService.WaterDiameterGetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
