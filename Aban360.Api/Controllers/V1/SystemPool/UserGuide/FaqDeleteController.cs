using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.UserGuide
{
    [Route("v1/faq")]
    public class FaqDeleteController : BaseController
    {
        private readonly IDeleteFaqHandler _DeleteHandler;

        public FaqDeleteController(IDeleteFaqHandler DeleteHandler)
        {
            _DeleteHandler = DeleteHandler;
            _DeleteHandler.NotNull(nameof(_DeleteHandler));
        }

        [Route("Delete/{id}")]
        [HttpPost, HttpDelete]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {           
           await _DeleteHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
