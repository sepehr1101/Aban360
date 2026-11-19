using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Processing.Services;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill")]
    public class GenerateBillController : BaseController
    {
        private readonly IBillGenerationService _billGenerationService;
        public GenerateBillController(IBillGenerationService billGenerationService)
        {
            _billGenerationService = billGenerationService;
            _billGenerationService.NotNull(nameof(billGenerationService));
        }

        [HttpPost, HttpGet]
        [Route("issue")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Generate(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            NewBillOutputDto result = await _billGenerationService.GenerateBillAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
