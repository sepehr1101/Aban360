using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/zarib-c")]
    public class ZaribCDeleteController : BaseController
    {
        private readonly IZaribCDeleteHandler _zaribCDeleteHandler;
        public ZaribCDeleteController(IZaribCDeleteHandler zaribCDeleteHandler)
        {
            _zaribCDeleteHandler = zaribCDeleteHandler;
            _zaribCDeleteHandler.NotNull(nameof(zaribCDeleteHandler));
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _zaribCDeleteHandler.Handle(id, cancellationToken);
            return Ok(id);
        }
    }
}
