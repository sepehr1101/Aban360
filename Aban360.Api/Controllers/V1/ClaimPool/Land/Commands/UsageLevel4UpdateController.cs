using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v4/usage-level-4")]
    public class UsageLevel4UpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel4UpdateHandler _usageLevel4UpdateHandler;
        public UsageLevel4UpdateController(
            IUnitOfWork uow,
            IUsageLevel4UpdateHandler usageLevel4UpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel4UpdateHandler = usageLevel4UpdateHandler;
            _usageLevel4UpdateHandler.NotNull(nameof(usageLevel4UpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel4UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
