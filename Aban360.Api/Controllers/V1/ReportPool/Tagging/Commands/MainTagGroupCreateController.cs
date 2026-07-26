using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.MainTagGroupging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupCreateController : BaseController
    {
        private readonly IMainTagGroupCreateHandler _createHandler;

        public MainTagGroupCreateController(IMainTagGroupCreateHandler createHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(createHandler));
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MainTagGroupInsertInputDto dto)
        {
            await _createHandler.Handle(dto);
            return Ok(dto);
        }
    }
}
