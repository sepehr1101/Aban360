using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/zarib-c")]
    public class ZaribCGetController : BaseController
    {
        private readonly IZaribCGetHandler _zaribCGetHandler;
        public ZaribCGetController(IZaribCGetHandler zaribCGetHandler)
        {
            _zaribCGetHandler = zaribCGetHandler;
            _zaribCGetHandler.NotNull(nameof(zaribCGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            ZaribCQueryDto result = await _zaribCGetHandler.Handle(fromDateJalali, toDateJalali, cancellationToken);
            return Ok(result);
        }
    }
}
