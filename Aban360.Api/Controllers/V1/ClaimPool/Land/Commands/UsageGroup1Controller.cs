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
    [Route("v1/usage-group1")]
    public class UsageGroup1Controller : BaseController
    {
        private readonly IUsageGroup1InsertHandler _usageGroup1InsertHandler;
        private readonly IUsageGroup1RemoveHandler _usageGroup1RemoveHandler;
        private readonly IUsageGroup1UpdateHandler _usageGroup1UpdateHandler;
        private readonly IUsageGroup1GetAllHandler _usageGroup1GetAllHandler;
        private readonly IUsageGroup1GetByIdHandler _usageGroup1GetByIdHandler;
        public UsageGroup1Controller(
                 IUsageGroup1InsertHandler usageGroup1InsertHandler,
                 IUsageGroup1RemoveHandler usageGroup1RemoveHandler,
                 IUsageGroup1UpdateHandler usageGroup1UpdateHandler,
                 IUsageGroup1GetAllHandler usageGroup1GetAllHandle,
                 IUsageGroup1GetByIdHandler usageGroup1GetByIdHandler)
        {
            _usageGroup1InsertHandler = usageGroup1InsertHandler;
            _usageGroup1InsertHandler.NotNull(nameof(usageGroup1InsertHandler));

            _usageGroup1RemoveHandler = usageGroup1RemoveHandler;
            _usageGroup1RemoveHandler.NotNull(nameof(usageGroup1RemoveHandler));

            _usageGroup1UpdateHandler = usageGroup1UpdateHandler;
            _usageGroup1UpdateHandler.NotNull(nameof(usageGroup1UpdateHandler));

            _usageGroup1GetAllHandler = usageGroup1GetAllHandle;
            _usageGroup1GetAllHandler.NotNull(nameof(usageGroup1GetAllHandle));

            _usageGroup1GetByIdHandler = usageGroup1GetByIdHandler;
            _usageGroup1GetByIdHandler.NotNull(nameof(usageGroup1GetByIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1InsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] UsageGroup1InsertDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup1InsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _usageGroup1RemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageGroup1UpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup1UpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            UsageGroup1GetDto result = await _usageGroup1GetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UsageGroup1GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup1GetDto> result = await _usageGroup1GetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
