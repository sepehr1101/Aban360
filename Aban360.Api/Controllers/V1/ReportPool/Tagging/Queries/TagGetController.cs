using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Implementations;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging
{
    [Route("v1/tag")]
    public class TagGetController : BaseController
    {
        private readonly IGetTagHandler _getHandler;

        public TagGetController(IGetTagHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TagDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _getHandler.HandleAll();
            return Ok(tags);
        }
    }
}
