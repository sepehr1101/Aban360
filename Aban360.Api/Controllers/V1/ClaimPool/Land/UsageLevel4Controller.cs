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
    [Route("v1/usage-level-4")]
    public class UsageLevel4Controller : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsageLevel4CreateHandler _usageLevel4CreateHandler;
        private readonly IUsageLevel4UpdateHandler _usageLevel4UpdateHandler;
        private readonly IUsageLevel4DeleteHandler _usageLevel4DeleteHandler;
        private readonly IUsageLevel4GetSingleHandler _usageLevel4GetSingleHandler;
        private readonly IUsageLevel4GetAllHandler _usageLevel4GetAllHandler;

        public UsageLevel4Controller(
            IUnitOfWork uow,
            IUsageLevel4CreateHandler usageLevel4CreateHandler,
            IUsageLevel4UpdateHandler usageLevel4UpdateHandler,
            IUsageLevel4DeleteHandler usageLevel4DeleteHandler,
            IUsageLevel4GetSingleHandler usageLevel4GetSingleHandler,
            IUsageLevel4GetAllHandler usageLevel4GetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel4CreateHandler = usageLevel4CreateHandler;
            _usageLevel4CreateHandler.NotNull(nameof(usageLevel4CreateHandler));

            _usageLevel4UpdateHandler = usageLevel4UpdateHandler;
            _usageLevel4UpdateHandler.NotNull(nameof(usageLevel4UpdateHandler));

            _usageLevel4DeleteHandler = usageLevel4DeleteHandler;
            _usageLevel4DeleteHandler.NotNull(nameof(usageLevel4DeleteHandler));

            _usageLevel4GetSingleHandler = usageLevel4GetSingleHandler;
            _usageLevel4GetSingleHandler.NotNull(nameof(usageLevel4GetSingleHandler));

            _usageLevel4GetAllHandler = usageLevel4GetAllHandler;
            _usageLevel4GetAllHandler.NotNull(nameof(usageLevel4GetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel4CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UsageLevel4CreateDto createDto, CancellationToken cancellationToken)
        {
            await _usageLevel4CreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _usageLevel4UpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel4DeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageLevel4DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _usageLevel4DeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel4GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel4s = await _usageLevel4GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel4s);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel4GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel4s = await _usageLevel4GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel4s);
        }
    }
}
