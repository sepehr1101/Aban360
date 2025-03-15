using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/usage")]
    public class UsageDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageDeleteHandler _usageHandler;
        public UsageDeleteController(
            IUnitOfWork uow,
            IUsageDeleteHandler usageHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageHandler = usageHandler;
            _usageHandler.NotNull(nameof(_usageHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
