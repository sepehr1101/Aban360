using Aban360.ClaimPool.Application.Features.Land.Services;
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
    [Route("v1/usage-level-2")]
    public class UsageLevel2Controller : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel2Service _service;

        public UsageLevel2Controller(IUnitOfWork uow, IUsageLevel2Service service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel2CreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2DeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageLevel2DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel2s = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(usageLevel2s);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel2GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel2s = await _service.GetAllAsync(cancellationToken);
            return Ok(usageLevel2s);
        }
    }
}
