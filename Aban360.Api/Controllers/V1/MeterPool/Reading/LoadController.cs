using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading;
using Aban360.MeterPool.Domain.Features.Reading;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading/load")]
    public class LoadController : BaseController
    {
        private readonly ILoad _load;

        public LoadController(ILoad load)
        {
            _load = load;
            _load.NotNull(nameof(load));
        }

        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IReadOnlyCollection<ReadingLoadDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Load(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ReadingLoadDto> readings = await _load.Handle(cancellationToken);
            return Ok(readings);
        }
    }
}
