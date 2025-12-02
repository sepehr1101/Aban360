using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/s")]
    public class SUpdateController : BaseController
    {
        private readonly ISUpdateHandler _sUpdateHandler;
        public SUpdateController(ISUpdateHandler sUpdateHandler)
        {
            _sUpdateHandler = sUpdateHandler;
            _sUpdateHandler.NotNull(nameof(sUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(SUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _sUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
