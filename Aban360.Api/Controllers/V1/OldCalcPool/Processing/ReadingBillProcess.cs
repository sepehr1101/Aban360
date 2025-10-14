using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.SaveReading;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/reading-bill")]
    public class ReadingBillProcess : BaseController
    {
        private readonly IWaterCalculationSaveHandler _saveHandler;
        public ReadingBillProcess(IWaterCalculationSaveHandler saveHandler)
        {
            _saveHandler = saveHandler;
            _saveHandler.NotNull(nameof(saveHandler));
        }

        [HttpPost, HttpGet]
        [Route("test-by-current-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> ReadingBill(IEnumerable<ReadingBillInputDto> inputDto, int mamorCode, CancellationToken cancellationToken)
        {
            await _saveHandler.Handle(inputDto, mamorCode, cancellationToken);
            return Ok(mamorCode);
        }
    }
}
