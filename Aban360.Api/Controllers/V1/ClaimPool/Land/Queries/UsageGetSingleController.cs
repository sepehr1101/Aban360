using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/usage")]
    public class UsageGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageGetSingleHandler _usageHandler;
        public UsageGetSingleController(
            IUnitOfWork uow,
            IUsageGetSingleHandler usageHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageHandler = usageHandler;
            _usageHandler.NotNull(nameof(_usageHandler));
        }

        [HttpGet ,HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> Create(short id, CancellationToken cancellationToken)
        {
            var usage = await _usageHandler.Handle(id, cancellationToken);
            return Ok(usage);
        }
    }
}
