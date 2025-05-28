using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/tag")]
    public class SpecialConditionTagsInfoController : BaseController
    {
        private readonly ISpecialConditionTagsInfoGetHandler _specialConditionTagsInfoHandle;
        public SpecialConditionTagsInfoController(
            ISpecialConditionTagsInfoGetHandler specialConditionTagsInfoHandle)
        {
            _specialConditionTagsInfoHandle = specialConditionTagsInfoHandle;
            _specialConditionTagsInfoHandle.NotNull(nameof(specialConditionTagsInfoHandle));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SpecialConditionTagsInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info( SearchInput searchInput,CancellationToken cancellationToken)
        {
            SpecialConditionTagsInfoDto summary = await _specialConditionTagsInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
