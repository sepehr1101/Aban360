using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V2.ClaimPool.Land.Commands
{
    [Route("v2/usage-group2")]
    public class UsageGroup2Controller : BaseController
    {
        private readonly IUsageGroup2InsertHandler _usageGroup2InsertHandler;
        private readonly IUsageGroup2RemoveHandler _usageGroup2RemoveHandler;
        private readonly IUsageGroup2UpdateHandler _usageGroup2UpdateHandler;
        private readonly IUsageGroup2GetAllHandler _usageGroup2GetAllHandler;
        private readonly IUsageGroup2GetByIdHandler _usageGroup2GetByIdHandler;
        public UsageGroup2Controller(
                 IUsageGroup2InsertHandler usageGroup2InsertHandler,
                 IUsageGroup2RemoveHandler usageGroup2RemoveHandler,
                 IUsageGroup2UpdateHandler usageGroup2UpdateHandler,
                 IUsageGroup2GetAllHandler usageGroup2GetAllHandle,
                 IUsageGroup2GetByIdHandler usageGroup2GetByIdHandler)
        {
            _usageGroup2InsertHandler = usageGroup2InsertHandler;
            _usageGroup2InsertHandler.NotNull(nameof(usageGroup2InsertHandler));

            _usageGroup2RemoveHandler = usageGroup2RemoveHandler;
            _usageGroup2RemoveHandler.NotNull(nameof(usageGroup2RemoveHandler));

            _usageGroup2UpdateHandler = usageGroup2UpdateHandler;
            _usageGroup2UpdateHandler.NotNull(nameof(usageGroup2UpdateHandler));

            _usageGroup2GetAllHandler = usageGroup2GetAllHandle;
            _usageGroup2GetAllHandler.NotNull(nameof(usageGroup2GetAllHandle));

            _usageGroup2GetByIdHandler = usageGroup2GetByIdHandler;
            _usageGroup2GetByIdHandler.NotNull(nameof(usageGroup2GetByIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup2InsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] UsageGroup2InsertDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup2InsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _usageGroup2RemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup2UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageGroup2UpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup2UpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup2GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            UsageGroup2GetDto result = await _usageGroup2GetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UsageGroup2GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup2GetDto> result = await _usageGroup2GetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
