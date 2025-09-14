using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/tag-group")]
    public class TagGroupCreateController : BaseController
    {
        private readonly ICreateTagGroupHandler _createHandler;

        public TagGroupCreateController(ICreateTagGroupHandler createHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(createHandler));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateTagGroupDto>), StatusCodes.Status200OK)]
        [Route("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateTagGroupDto dto)
        {
            var id = await _createHandler.Handle(dto);
            return Ok(dto);
        }
    }
}
