using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v3/usage-group3")]
    public class UsageGroup3Controller : BaseController
    {
        private readonly IUsageGroup3InsertHandler _usageGroup3InsertHandler;
        private readonly IUsageGroup3RemoveHandler _usageGroup3RemoveHandler;
        private readonly IUsageGroup3UpdateHandler _usageGroup3UpdateHandler;
        private readonly IUsageGroup3GetAllHandler _usageGroup3GetAllHandler;
        private readonly IUsageGroup3GetByIdHandler _usageGroup3GetByIdHandler;
        public UsageGroup3Controller(
                 IUsageGroup3InsertHandler usageGroup3InsertHandler,
                 IUsageGroup3RemoveHandler usageGroup3RemoveHandler,
                 IUsageGroup3UpdateHandler usageGroup3UpdateHandler,
                 IUsageGroup3GetAllHandler usageGroup3GetAllHandle,
                 IUsageGroup3GetByIdHandler usageGroup3GetByIdHandler)
        {
            _usageGroup3InsertHandler = usageGroup3InsertHandler;
            _usageGroup3InsertHandler.NotNull(nameof(usageGroup3InsertHandler));

            _usageGroup3RemoveHandler = usageGroup3RemoveHandler;
            _usageGroup3RemoveHandler.NotNull(nameof(usageGroup3RemoveHandler));

            _usageGroup3UpdateHandler = usageGroup3UpdateHandler;
            _usageGroup3UpdateHandler.NotNull(nameof(usageGroup3UpdateHandler));

            _usageGroup3GetAllHandler = usageGroup3GetAllHandle;
            _usageGroup3GetAllHandler.NotNull(nameof(usageGroup3GetAllHandle));

            _usageGroup3GetByIdHandler = usageGroup3GetByIdHandler;
            _usageGroup3GetByIdHandler.NotNull(nameof(usageGroup3GetByIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup3InsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] UsageGroup3InsertDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup3InsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _usageGroup3RemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup3UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageGroup3UpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup3UpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup3GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            UsageGroup3GetDto result = await _usageGroup3GetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UsageGroup3GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup3GetDto> result = await _usageGroup3GetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
