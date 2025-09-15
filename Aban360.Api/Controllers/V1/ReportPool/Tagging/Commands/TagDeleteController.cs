using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/tag")]
    public class TagDeleteController : BaseController
    {
        private readonly IDeleteTagHandler _deleteHandler;

        public TagDeleteController(IDeleteTagHandler deleteHandler)
        {
            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(deleteHandler));
        }

        [HttpPost, HttpDelete]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deleteHandler.Handle(id);
            if (!result)
                return NotFound();
            return Ok(new { Id = id });
        }
    }
}
