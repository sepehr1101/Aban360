using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.UserGuide
{
    [Route("v1/faq")]
    public class FaqCreateController : BaseController
    {
        private readonly ICreateFaqHandler _createHandler;

        public FaqCreateController(ICreateFaqHandler createHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(_createHandler));
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FaqDto faq, CancellationToken cancellationToken)
        {
            faq.NotNull(nameof(faq));
            int id = await _createHandler.Handle(faq, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
