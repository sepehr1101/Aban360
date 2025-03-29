using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/usage-level-1")]
    public class UsageLevel1UpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel1UpdateHandler _usageLevel1UpdateHandler;
        public UsageLevel1UpdateController(
            IUnitOfWork uow,
            IUsageLevel1UpdateHandler usageLevel1UpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel1UpdateHandler = usageLevel1UpdateHandler;
            _usageLevel1UpdateHandler.NotNull(nameof(usageLevel1UpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel1UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
