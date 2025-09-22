using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/zarib")]
    public class ZaribGetController : BaseController
    {
        private readonly IZaribGetHandler _zaribGetHandler;
        public ZaribGetController(IZaribGetHandler zaribGetHandler)
        {
            _zaribGetHandler = zaribGetHandler;
            _zaribGetHandler.NotNull(nameof(zaribGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            ZaribGetDto result = await _zaribGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
