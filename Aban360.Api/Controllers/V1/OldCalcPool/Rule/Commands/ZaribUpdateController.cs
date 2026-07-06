using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/zarib")]
    public class ZaribUpdateController : BaseController
    {
        private readonly IZaribUpdateHandler _zaribUpdateHandler;
        public ZaribUpdateController(IZaribUpdateHandler zaribUpdateHandler)
        {
            _zaribUpdateHandler = zaribUpdateHandler;
            _zaribUpdateHandler.NotNull(nameof(zaribUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(ZaribUpdateInputDto UpdateDto, CancellationToken cancellationToken)
        {
            await _zaribUpdateHandler.Handle(UpdateDto, CurrentUser, cancellationToken);
            return Ok(UpdateDto);
        }
    }
}
