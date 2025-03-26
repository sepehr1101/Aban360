using Aban360.Api.Controllers.V1;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban460.Api.Controllers.V1.UserPool.TimeTable.Commands
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
