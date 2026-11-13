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

namespace Aban360.Api.Controllers.V1.BeforeBatch151.ClaimPool.Land
{
    [Route("v1/usage")]
    public class UsageController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageCreateHandler _usageCreateHandler;
        private readonly IUsageUpdateHandler _usageUpdateHandler;
        private readonly IUsageDeleteHandler _usageDeleteHandler;
        private readonly IUsageGetSingleHandler _usageGetSingleHandler;
        private readonly IUsageGetAllHandler _usageGetAllHandler;

        public UsageController(
            IUnitOfWork uow,
            IUsageCreateHandler usageCreateHandler,
            IUsageUpdateHandler usageUpdateHandler,
            IUsageDeleteHandler usageDeleteHandler,
            IUsageGetSingleHandler usageGetSingleHandler,
            IUsageGetAllHandler usageGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageCreateHandler = usageCreateHandler;
            _usageCreateHandler.NotNull(nameof(_usageCreateHandler));

            _usageUpdateHandler = usageUpdateHandler;
            _usageUpdateHandler.NotNull(nameof(_usageUpdateHandler));

            _usageDeleteHandler = usageDeleteHandler;
            _usageDeleteHandler.NotNull(nameof(_usageDeleteHandler));

            _usageGetSingleHandler = usageGetSingleHandler;
            _usageGetSingleHandler.NotNull(nameof(_usageGetSingleHandler));

            _usageGetAllHandler = usageGetAllHandler;
            _usageGetAllHandler.NotNull(nameof(_usageGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageCreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet ,HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(short id, CancellationToken cancellationToken)
        {
            UsageGetDto usage = await _usageGetSingleHandler.Handle(id, cancellationToken);
            return Ok(usage);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<UsageGetDto> usages = await _usageGetAllHandler.Handle(cancellationToken);
            return Ok(usages);
        }
    }
}
