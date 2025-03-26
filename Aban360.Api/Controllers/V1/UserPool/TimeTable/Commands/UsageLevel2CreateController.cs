using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V2.UserPool.TimeTable.Commands
{
    [Route("v2/usage-level-2")]
    public class UsageLevel2CreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel2CreateHandler _usageLevel2CreateHandler;
        public UsageLevel2CreateController(
            IUnitOfWork uow,
            IUsageLevel2CreateHandler usageLevel2CreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel2CreateHandler = usageLevel2CreateHandler;
            _usageLevel2CreateHandler.NotNull(nameof(usageLevel2CreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel2CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel2CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
