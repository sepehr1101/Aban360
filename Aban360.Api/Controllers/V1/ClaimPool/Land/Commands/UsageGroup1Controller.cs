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

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/usage-group1")]
    public class UsageGroup1Controller : BaseController
    {
        private readonly IUsageGroup1Service _usageGroup1Service;
        public UsageGroup1Controller(IUsageGroup1Service usageGroup1Service)
        {
            _usageGroup1Service = usageGroup1Service;
            _usageGroup1Service.NotNull(nameof(usageGroup1Service));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1InsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] UsageGroup1InsertDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup1Service.InsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("insert-duplicate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1DuplicateInsertOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertDuplicate([FromBody] UsageGroup1DuplicateInsetInputDto inputDto, CancellationToken cancellationToken)
        {
            UsageGroup1DuplicateInsertOutputDto result = await _usageGroup1Service.DuplicateInsertAsync(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _usageGroup1Service.RemoveAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageGroup1UpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _usageGroup1Service.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            UsageGroup1GetDto result = await _usageGroup1Service.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UsageGroup1GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup1GetDto> result = await _usageGroup1Service.GetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
