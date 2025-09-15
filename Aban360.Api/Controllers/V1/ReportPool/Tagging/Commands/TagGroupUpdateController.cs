using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Implementations;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/tag-group")]
    public class TagGroupUpdateController : BaseController
    {
        private readonly IUpdateTagGroupHandler _updateHandler;

        public TagGroupUpdateController(IUpdateTagGroupHandler updateHandler)
        {
            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(updateHandler));
        }

        [HttpPatch, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UpdateTagGroupDto>), StatusCodes.Status200OK)]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTagGroupDto dto)
        {
            var result = await _updateHandler.Handle(dto);
            if (!result)
                return NotFound();
            return Ok(dto);
        }
    }
}
