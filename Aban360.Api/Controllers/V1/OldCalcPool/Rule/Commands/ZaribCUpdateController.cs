using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/zarib-c")]
    public class ZaribCUpdateController : BaseController
    {
        private readonly IZaribCUpdateHandler _zaribCUpdateHandler;
        public ZaribCUpdateController(IZaribCUpdateHandler zaribCUpdateHandler)
        {
            _zaribCUpdateHandler = zaribCUpdateHandler;
            _zaribCUpdateHandler.NotNull(nameof(zaribCUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(ZaribCUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _zaribCUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
