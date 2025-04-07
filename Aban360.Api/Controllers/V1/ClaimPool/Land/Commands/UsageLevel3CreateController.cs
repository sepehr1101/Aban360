using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/usage-level-3")]
    public class UsageLevel3CreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel3CreateHandler _usageLevel3CreateHandler;
        public UsageLevel3CreateController(
            IUnitOfWork uow,
            IUsageLevel3CreateHandler usageLevel3CreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel3CreateHandler = usageLevel3CreateHandler;
            _usageLevel3CreateHandler.NotNull(nameof(usageLevel3CreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel3CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel3CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel3CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
