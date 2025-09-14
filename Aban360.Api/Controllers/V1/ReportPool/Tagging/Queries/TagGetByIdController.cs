using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Implementations;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging
{
    [Route("v1/tag")]
    public class TagGetByIdController : BaseController
    {
        private readonly IGetTagHandler _getHandler;

        public TagGetByIdController(GetTagHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TagDto>), StatusCodes.Status200OK)]
        [Route("get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            TagDto tag = await _getHandler.Handle(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }
    }
}
