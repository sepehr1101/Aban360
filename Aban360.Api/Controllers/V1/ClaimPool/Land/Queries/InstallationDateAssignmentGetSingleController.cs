using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/installation-date-assignment")]
    public class InstallationDateAssignmentGetSingleController : ControllerBase
    {
        private readonly IInstallationDateAssignmentGetHandler _getSingleHandler;
        public InstallationDateAssignmentGetSingleController(IInstallationDateAssignmentGetHandler getSingleHandler)
        {
            _getSingleHandler = getSingleHandler;
            _getSingleHandler.NotNull(nameof(_getSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationDateAssignmentGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(string billId, CancellationToken cancellationToken)
        {
            InstallationDateAssignmentGetDto installationDateAssignment = await _getSingleHandler.Handle(billId, cancellationToken);
            return Ok(installationDateAssignment);
        }
    }
}
