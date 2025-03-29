using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/usage-level-1")]
    public class UsageLevel1CreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel1CreateHandler _usageLevel1CreateHandler;
        public UsageLevel1CreateController(
            IUnitOfWork uow,
            IUsageLevel1CreateHandler usageLevel1CreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel1CreateHandler = usageLevel1CreateHandler;
            _usageLevel1CreateHandler.NotNull(nameof(usageLevel1CreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel1CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel1CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel1CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
