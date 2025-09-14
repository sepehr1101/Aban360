using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Implementations;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging
{
    [Route("v1/tag-group")]
    public class TagGroupGetByIdController : BaseController
    {
        private readonly IGetTagGroupHandler _getHandler;

        public TagGroupGetByIdController(GetTagGroupHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TagGroupDto>), StatusCodes.Status200OK)]
        [Route("by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            TagGroupDto group = await _getHandler.Handle(id);
            if (group == null)
                return NotFound();
            return Ok(group);
        }

    }
}
