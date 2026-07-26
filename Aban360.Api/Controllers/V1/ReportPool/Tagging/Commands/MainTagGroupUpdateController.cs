using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupUpdateController : BaseController
    {
        private readonly IMainTagGroupUpdateHandler _updateHandler;

        public MainTagGroupUpdateController(IMainTagGroupUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(updateHandler));
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MainTagGroupUpdateDto input, CancellationToken cancellationToken)
        {
            await _updateHandler.Handle(input);
            return Ok(input);
        }
    }
}
