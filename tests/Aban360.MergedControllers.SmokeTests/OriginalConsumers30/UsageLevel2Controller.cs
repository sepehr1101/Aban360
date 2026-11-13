// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Land;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/usage-level-2")]
    public class UsageLevel2Controller : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel2CreateHandler _usageLevel2CreateHandler;
        private readonly IUsageLevel2UpdateHandler _usageLevel2UpdateHandler;
        private readonly IUsageLevel2DeleteHandler _usageLevel2DeleteHandler;
        private readonly IUsageLevel2GetSingleHandler _usageLevel2GetSingleHandler;
        private readonly IUsageLevel2GetAllHandler _usageLevel2GetAllHandler;

        public UsageLevel2Controller(
            IUnitOfWork uow,
            IUsageLevel2CreateHandler usageLevel2CreateHandler,
            IUsageLevel2UpdateHandler usageLevel2UpdateHandler,
            IUsageLevel2DeleteHandler usageLevel2DeleteHandler,
            IUsageLevel2GetSingleHandler usageLevel2GetSingleHandler,
            IUsageLevel2GetAllHandler usageLevel2GetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel2CreateHandler = usageLevel2CreateHandler;
            _usageLevel2CreateHandler.NotNull(nameof(usageLevel2CreateHandler));

            _usageLevel2UpdateHandler = usageLevel2UpdateHandler;
            _usageLevel2UpdateHandler.NotNull(nameof(usageLevel2UpdateHandler));

            _usageLevel2DeleteHandler = usageLevel2DeleteHandler;
            _usageLevel2DeleteHandler.NotNull(nameof(usageLevel2DeleteHandler));

            _usageLevel2GetSingleHandler = usageLevel2GetSingleHandler;
            _usageLevel2GetSingleHandler.NotNull(nameof(usageLevel2GetSingleHandler));

            _usageLevel2GetAllHandler = usageLevel2GetAllHandler;
            _usageLevel2GetAllHandler.NotNull(nameof(usageLevel2GetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel2UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel2s = await _usageLevel2GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel2s);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel2GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel2s = await _usageLevel2GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel2s);
        }
    }
}
