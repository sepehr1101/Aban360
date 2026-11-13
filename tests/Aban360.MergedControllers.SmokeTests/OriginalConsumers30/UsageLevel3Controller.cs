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
    [Route("v1/usage-level-3")]
    public class UsageLevel3Controller : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel3CreateHandler _usageLevel3CreateHandler;
        private readonly IUsageLevel3UpdateHandler _usageLevel3UpdateHandler;
        private readonly IUsageLevel3DeleteHandler _usageLevel3DeleteHandler;
        private readonly IUsageLevel3GetSingleHandler _usageLevel3GetSingleHandler;
        private readonly IUsageLevel3GetAllHandler _usageLevel3GetAllHandler;

        public UsageLevel3Controller(
            IUnitOfWork uow,
            IUsageLevel3CreateHandler usageLevel3CreateHandler,
            IUsageLevel3UpdateHandler usageLevel3UpdateHandler,
            IUsageLevel3DeleteHandler usageLevel3DeleteHandler,
            IUsageLevel3GetSingleHandler usageLevel3GetSingleHandler,
            IUsageLevel3GetAllHandler usageLevel3GetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel3CreateHandler = usageLevel3CreateHandler;
            _usageLevel3CreateHandler.NotNull(nameof(usageLevel3CreateHandler));

            _usageLevel3UpdateHandler = usageLevel3UpdateHandler;
            _usageLevel3UpdateHandler.NotNull(nameof(usageLevel3UpdateHandler));

            _usageLevel3DeleteHandler = usageLevel3DeleteHandler;
            _usageLevel3DeleteHandler.NotNull(nameof(usageLevel3DeleteHandler));

            _usageLevel3GetSingleHandler = usageLevel3GetSingleHandler;
            _usageLevel3GetSingleHandler.NotNull(nameof(usageLevel3GetSingleHandler));

            _usageLevel3GetAllHandler = usageLevel3GetAllHandler;
            _usageLevel3GetAllHandler.NotNull(nameof(usageLevel3GetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel3CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel3CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel3CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel3UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel3UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel3DeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageLevel3DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageLevel3DeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel3GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel3s = await _usageLevel3GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel3s);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel3GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel3s = await _usageLevel3GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel3s);
        }
    }
}
