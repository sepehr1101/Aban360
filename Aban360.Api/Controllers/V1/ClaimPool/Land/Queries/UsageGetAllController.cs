using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/usage")]
    public class UsageGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageGetAllHandler _usageHandler;
        public UsageGetAllController(
            IUnitOfWork uow,
            IUsageGetAllHandler usageHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageHandler = usageHandler;
            _usageHandler.NotNull(nameof(_usageHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usages = await _usageHandler.Handle(cancellationToken);
            return Ok(usages);
        }
    }
}
