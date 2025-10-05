using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.UserGuide
{
    [Route("v1/faq")]
    public class FaqUpdateController : BaseController
    {
        private readonly IUpdateFaqHandler _UpdateHandler;

        public FaqUpdateController(IUpdateFaqHandler UpdateHandler)
        {
            _UpdateHandler = UpdateHandler;
            _UpdateHandler.NotNull(nameof(_UpdateHandler));
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] FaqDto faq, CancellationToken cancellationToken)
        {
            faq.NotNull(nameof(faq));
            int id = await _UpdateHandler.Handle(faq, cancellationToken);
            return Ok(id);
        }
    }
}
