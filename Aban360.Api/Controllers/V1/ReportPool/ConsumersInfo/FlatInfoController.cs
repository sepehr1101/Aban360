using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/flat")]
    public class FlatInfoController : BaseController
    {
        private readonly IFlatInfoGetHandler _FlatInfoHandle;
        public FlatInfoController(
            IFlatInfoGetHandler FlatInfoHandle)
        {
            _FlatInfoHandle = FlatInfoHandle;
            _FlatInfoHandle.NotNull(nameof(FlatInfoHandle));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info( SearchInput searchInput,CancellationToken cancellationToken)
        {
            FlatInfoDto summary = await _FlatInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }

    }
}
