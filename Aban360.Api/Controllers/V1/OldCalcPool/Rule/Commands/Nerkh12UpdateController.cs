using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/nerkh-12")]
    public class Nerkh12UpdateController : BaseController
    {
        private readonly INerkhUpdateHandler _nerkhUpdateHandler;
        public Nerkh12UpdateController(INerkhUpdateHandler nerkhUpdateHandler)
        {
            _nerkhUpdateHandler = nerkhUpdateHandler;
            _nerkhUpdateHandler.NotNull(nameof(nerkhUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(NerkhUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            await _nerkhUpdateHandler.Handle(UpdateDto, 12, cancellationToken);
            return Ok(UpdateDto);
        }
    }
}
