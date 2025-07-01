using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/branch")]
    public class BranchSpecificationInfoController : BaseController
    {
        private readonly IBranchSpecificationInfoGetHandler _branchSpecificationInfoHandle;
        public BranchSpecificationInfoController(
            IBranchSpecificationInfoGetHandler branchSpecificationInfoHandle)
        {
            _branchSpecificationInfoHandle = branchSpecificationInfoHandle;
            _branchSpecificationInfoHandle.NotNull(nameof(branchSpecificationInfoHandle));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BranchSpecificationInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info( SearchInput searchInput,CancellationToken cancellationToken)
        {
            BranchSpecificationInfoDto summary = await _branchSpecificationInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }

    }
}
