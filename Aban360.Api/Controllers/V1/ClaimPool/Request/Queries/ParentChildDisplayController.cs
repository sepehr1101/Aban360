using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class ParentChildDisplayController : BaseController
    {
        private readonly IMotherChildRequestService _motherChildRequestService;
        public ParentChildDisplayController(IMotherChildRequestService motherChildRequestService)
        {
            _motherChildRequestService = motherChildRequestService;
            _motherChildRequestService.NotNull(nameof(motherChildRequestService));
        }

        [HttpGet, HttpPost]
        [Route("display-parent-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(SearchNumericInput input, CancellationToken cancellationToken)
        {
            MotherChildOutputDto result = await _motherChildRequestService.GetByTrackNumberAsync(input.Input, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("display-parent-child/{trackNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByQuery(int trackNumber, CancellationToken cancellationToken)
        {
            MotherChildOutputDto result = await _motherChildRequestService.GetByTrackNumberAsync(trackNumber, cancellationToken);
            return Ok(result);
        }
    }
}
