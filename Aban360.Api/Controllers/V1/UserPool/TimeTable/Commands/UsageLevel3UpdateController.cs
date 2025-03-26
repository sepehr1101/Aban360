using Aban360.Api.Controllers.V1;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V3.UserPool.TimeTable.Commands
{
    [Route("v3/usage-level-3")]
    public class UsageLevel3UpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel3UpdateHandler _usageLevel3UpdateHandler;
        public UsageLevel3UpdateController(
            IUnitOfWork uow,
            IUsageLevel3UpdateHandler usageLevel3UpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel3UpdateHandler = usageLevel3UpdateHandler;
            _usageLevel3UpdateHandler.NotNull(nameof(usageLevel3UpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel3UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel3UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
