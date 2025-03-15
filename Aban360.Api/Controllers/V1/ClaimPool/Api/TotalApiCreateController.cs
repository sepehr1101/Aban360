using Aban360.ClaimPool.Application.Features.TotalApi.Handler;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Api
{
    [Route("v1/claim-pool")]
    public class TotalApiCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITotalApiCommandService _totalApiCommandService;
        public TotalApiCreateController(
            IUnitOfWork uow,
           ITotalApiCommandService totalApiCommandService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _totalApiCommandService = totalApiCommandService;
            _totalApiCommandService.NotNull(nameof(totalApiCommandService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TotalApiCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TotalApiCreateDto createDto, CancellationToken cancellationToken)
        {
            await _totalApiCommandService.Handle(createDto, cancellationToken);
            return Ok(createDto);
        }
    }
}
