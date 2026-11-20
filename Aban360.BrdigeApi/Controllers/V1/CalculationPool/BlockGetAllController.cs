using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using ISaleDictionariesService = global::Aban360.CalculationPool.Application.Features.Sale.Services.ISaleDictionariesService;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/block")]
    public class BlockGetAllController : BaseController
    {
        private readonly ISaleDictionariesService _saleDictionariesService;
        public BlockGetAllController(ISaleDictionariesService saleDictionariesService)
        {
            _saleDictionariesService = saleDictionariesService;
            _saleDictionariesService.NotNull(nameof(saleDictionariesService));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BlockGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<BlockGetDto> result = await _saleDictionariesService.BlockGetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
