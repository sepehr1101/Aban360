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

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/usage-level-1")]
    public class UsageLevel1Controller : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel1CreateHandler _usageLevel1CreateHandler;
        private readonly IUsageLevel1UpdateHandler _usageLevel1UpdateHandler;
        private readonly IUsageLevel1DeleteHandler _usageLevel1DeleteHandler;
        private readonly IUsageLevel1GetSingleHandler _usageLevel1GetSingleHandler;
        private readonly IUsageLevel1GetAllHandler _usageLevel1GetAllHandler;

        public UsageLevel1Controller(
            IUnitOfWork uow,
            IUsageLevel1CreateHandler usageLevel1CreateHandler,
            IUsageLevel1UpdateHandler usageLevel1UpdateHandler,
            IUsageLevel1DeleteHandler usageLevel1DeleteHandler,
            IUsageLevel1GetSingleHandler usageLevel1GetSingleHandler,
            IUsageLevel1GetAllHandler usageLevel1GetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel1CreateHandler = usageLevel1CreateHandler;
            _usageLevel1CreateHandler.NotNull(nameof(usageLevel1CreateHandler));

            _usageLevel1UpdateHandler = usageLevel1UpdateHandler;
            _usageLevel1UpdateHandler.NotNull(nameof(usageLevel1UpdateHandler));

            _usageLevel1DeleteHandler = usageLevel1DeleteHandler;
            _usageLevel1DeleteHandler.NotNull(nameof(usageLevel1DeleteHandler));

            _usageLevel1GetSingleHandler = usageLevel1GetSingleHandler;
            _usageLevel1GetSingleHandler.NotNull(nameof(usageLevel1GetSingleHandler));

            _usageLevel1GetAllHandler = usageLevel1GetAllHandler;
            _usageLevel1GetAllHandler.NotNull(nameof(usageLevel1GetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel1CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel1CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel1CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel1UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel1DeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageLevel1DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageLevel1DeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel1s = await _usageLevel1GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel1s);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel1GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel1s = await _usageLevel1GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel1s);
        }
    }
}
