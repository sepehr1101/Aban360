using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/removed-bill")]
    public class RemovedBillController : BaseController
    {
        private readonly IRemovedBillHandler _removedBillHandler;
        public RemovedBillController(IRemovedBillHandler removedBillHandler)
        {
            _removedBillHandler = removedBillHandler;
            _removedBillHandler.NotNull(nameof(removedBillHandler));
        }

        [HttpPost, HttpGet]
        [Route("removed")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RemovedBillInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Removed(RemovedBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _removedBillHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
    }
}
