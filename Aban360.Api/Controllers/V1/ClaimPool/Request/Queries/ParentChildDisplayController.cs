using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class ParentChildDisplayController : BaseController
    {
        private readonly IMotherChildGetByTrackNumberHandler _motherChildGetHandler;
        public ParentChildDisplayController(IMotherChildGetByTrackNumberHandler motherChildGetHandler)
        {
            _motherChildGetHandler = motherChildGetHandler;
            _motherChildGetHandler.NotNull(nameof(motherChildGetHandler));
        }

        [HttpGet]
        [Route("display-parent-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(SearchNumericInput input, CancellationToken cancellationToken)
        {
            MotherChildOutputDto result = await _motherChildGetHandler.Handle(input.Input, cancellationToken);
            return Ok(result);
        }
    }

}
