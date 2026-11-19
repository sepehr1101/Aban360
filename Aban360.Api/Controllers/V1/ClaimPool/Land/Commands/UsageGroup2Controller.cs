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
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V2.ClaimPool.Land.Commands
{
    [Route("v1/usage-group2")]
    public class UsageGroup2Controller : BaseController
    {
        private readonly IUsageGroup2Service _usageGroup2Service;
        public UsageGroup2Controller(IUsageGroup2Service usageGroup2Service)
        {
            _usageGroup2Service = usageGroup2Service;
            _usageGroup2Service.NotNull(nameof(usageGroup2Service));
        }

        [HttpPost, HttpGet]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup2InsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] UsageGroup2InsertDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup2Service.InsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _usageGroup2Service.RemoveAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup2UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageGroup2UpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup2Service.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup2GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            UsageGroup2GetDto result = await _usageGroup2Service.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get-by-group1/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UsageGroup2GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGroupqId(short id, CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup2GetDto> result = await _usageGroup2Service.GetByGroup1IdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UsageGroup2GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup2GetDto> result = await _usageGroup2Service.GetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
