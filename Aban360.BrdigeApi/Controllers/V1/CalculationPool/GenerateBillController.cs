using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill")]
    public class GenerateBillController : BaseController
    {
        private readonly IGenerateBillHandler _generageBillHandler;
        public GenerateBillController(IGenerateBillHandler generageBillHandler)
        {
            _generageBillHandler = generageBillHandler;
            _generageBillHandler.NotNull(nameof(generageBillHandler));
        }

        [HttpPost, HttpGet]
        [Route("issue")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Generate(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _generageBillHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}