using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/branch-history")]
    public class BranchHistoryInfoController : BaseController
    {
        private readonly IBranchHistoryInfoGetHandler _branchHistoryInfoHandle;
        public BranchHistoryInfoController(
            IBranchHistoryInfoGetHandler branchHistoryInfoHandle)
        {
            _branchHistoryInfoHandle = branchHistoryInfoHandle;
            _branchHistoryInfoHandle.NotNull(nameof(branchHistoryInfoHandle));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BranchHistoryInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info( SearchInput searchInput,CancellationToken cancellationToken)
        {
            BranchHistoryInfoDto summary = await _branchHistoryInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
