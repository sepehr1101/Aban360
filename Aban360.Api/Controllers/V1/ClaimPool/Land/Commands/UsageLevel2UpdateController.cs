using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/usage-level-2")]
    public class UsageLevel2UpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel2UpdateHandler _usageLevel2UpdateHandler;
        public UsageLevel2UpdateController(
            IUnitOfWork uow,
            IUsageLevel2UpdateHandler usageLevel2UpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel2UpdateHandler = usageLevel2UpdateHandler;
            _usageLevel2UpdateHandler.NotNull(nameof(usageLevel2UpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel2UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
