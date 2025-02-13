using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("usage")]
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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usage = await _usageHandler.Handle(cancellationToken);
            return Ok(usage);
        }
    }
}
