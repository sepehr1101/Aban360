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
        private readonly IFreeGenerateBillHandler _freeGenerateBillHandler;
        private readonly IGenerateBillWithPreDebtHandler _generateBillWithPreDebtHandler;
        public GenerateBillController(
            IGenerateBillHandler generateBillHandler,
            IFreeGenerateBillHandler freeGenerateBillHandler,
            IGenerateBillWithPreDebtHandler generateBillWithPreDebtHandler)
        {
            _generateBillHandler = generateBillHandler;
            _generateBillHandler.NotNull(nameof(generateBillHandler));

            _freeGenerateBillHandler = freeGenerateBillHandler;
            _freeGenerateBillHandler.NotNull(nameof(freeGenerateBillHandler));

            _generateBillWithPreDebtHandler = generateBillWithPreDebtHandler;
            _generateBillWithPreDebtHandler.NotNull(nameof(generateBillWithPreDebtHandler));
        }

        [HttpPost, HttpGet]
        [Route("calc")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            NewBillOutputDto result = await _generateBillHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("free")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FreeCalculation(FreeGenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            NewBillOutputDto result = await _freeGenerateBillHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("with-pre-debt")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> WithPreDebt(string billId, CancellationToken cancellationToken)
        {
            NewBillOutputDto result = await _generateBillWithPreDebtHandler.Handle(billId, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
