using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/s")]
    public class SDeleteController : BaseController
    {
        private readonly ISDeleteHandler _sDeleteHandler;
        public SDeleteController(ISDeleteHandler sDeleteHandler)
        {
            _sDeleteHandler = sDeleteHandler;
            _sDeleteHandler.NotNull(nameof(sDeleteHandler));
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _sDeleteHandler.Handle(id, cancellationToken);
            return Ok(id);
        }
    }
}
