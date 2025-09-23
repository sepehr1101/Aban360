using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/zarib-c")]
    public class ZaribCGetAllController : BaseController
    {
        private readonly IZaribCGetAllHandler _zaribCGetAllHandler;
        public ZaribCGetAllController(IZaribCGetAllHandler zaribCGetAllHandler)
        {
            _zaribCGetAllHandler = zaribCGetAllHandler;
            _zaribCGetAllHandler.NotNull(nameof(zaribCGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ZaribCQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribCQueryDto> result = await _zaribCGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
