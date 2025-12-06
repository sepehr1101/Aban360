using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/generate-bill")]
    public class GenerateBillController : BaseController
    {
        private readonly IGenerateBillHandler _generateBillHandler;
        public GenerateBillController(IGenerateBillHandler generateBillHandler)
        {
            _generateBillHandler = generateBillHandler;
            _generateBillHandler.NotNull(nameof(generateBillHandler));
        }

        [HttpPost, HttpGet]
        [Route("calc")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result =await _generateBillHandler.Handle(inputDto,cancellationToken);
            return Ok(result);
        }
    }
}
