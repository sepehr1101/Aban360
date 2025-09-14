using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Implementations;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/tag")]
    public class TagCreateController : BaseController
    {
        private readonly ICreateTagHandler _createHandler;

        public TagCreateController(CreateTagHandler createHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(createHandler));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateTagDto>), StatusCodes.Status200OK)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
        {
            var id = await _createHandler.Handle(dto);
            return Ok(dto);
        }
    }
}