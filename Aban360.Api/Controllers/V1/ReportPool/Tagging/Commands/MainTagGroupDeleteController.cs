using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupDeleteController : BaseController
    {
        private readonly IMainTagGroupDeleteHandler _deleteHandler;

        public MainTagGroupDeleteController(IMainTagGroupDeleteHandler deleteHandler)
        {
            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(deleteHandler));
        }

        [Route("delete/{id}")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _deleteHandler.Handle(id);
            return Ok(id);
        }
    }
}
