using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v4/usage-level-4")]
    public class UsageLevel4CreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel4CreateHandler _usageLevel4CreateHandler;
        public UsageLevel4CreateController(
            IUnitOfWork uow,
            IUsageLevel4CreateHandler usageLevel4CreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel4CreateHandler = usageLevel4CreateHandler;
            _usageLevel4CreateHandler.NotNull(nameof(usageLevel4CreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel4CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel4CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel4CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
