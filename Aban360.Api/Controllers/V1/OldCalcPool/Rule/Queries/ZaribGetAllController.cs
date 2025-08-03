using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/zarib")]
    public class ZaribGetAllController : BaseController
    {
        private readonly IZaribGetAllHandler _zaribGetAllHandler;
        public ZaribGetAllController(IZaribGetAllHandler zaribGetAllHandler)
        {
            _zaribGetAllHandler = zaribGetAllHandler;
            _zaribGetAllHandler.NotNull(nameof(zaribGetAllHandler));
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ZaribGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribGetDto> result = await _zaribGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
