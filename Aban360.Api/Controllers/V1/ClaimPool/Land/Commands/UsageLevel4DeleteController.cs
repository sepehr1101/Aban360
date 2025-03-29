using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v4/usage-level-4")]
    public class UsageLevel4DeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel4DeleteHandler _usageLevel4DeleteHandler;
        public UsageLevel4DeleteController(
            IUnitOfWork uow,
            IUsageLevel4DeleteHandler usageLevel4DeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel4DeleteHandler = usageLevel4DeleteHandler;
            _usageLevel4DeleteHandler.NotNull(nameof(usageLevel4DeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel4DeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageLevel4DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageLevel4DeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
