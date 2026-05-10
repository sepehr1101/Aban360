using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries
{
    [Route("v1/op-log")]
    public class OpLogGetController : BaseController
    {
        private readonly IOpLogQueryService _opLogQueryService;
        public OpLogGetController(IOpLogQueryService opLogQueryService)
        {
            _opLogQueryService = opLogQueryService;
            _opLogQueryService.NotNull(nameof(opLogQueryService));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<OpLogDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(OpLogGetDto inputDto, CancellationToken cancellation)
        {
            IEnumerable<OpLogDataDto> result = await _opLogQueryService.Get(inputDto);
            return Ok(result);
        }
    }
}
