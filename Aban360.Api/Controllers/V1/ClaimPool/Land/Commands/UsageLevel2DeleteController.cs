using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v2/usage-level-2")]
    public class UsageLevel2DeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel2DeleteHandler _usageLevel2DeleteHandler;
        public UsageLevel2DeleteController(
            IUnitOfWork uow,
            IUsageLevel2DeleteHandler usageLevel2DeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel2DeleteHandler = usageLevel2DeleteHandler;
            _usageLevel2DeleteHandler.NotNull(nameof(usageLevel2DeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2DeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageLevel2DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageLevel2DeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
