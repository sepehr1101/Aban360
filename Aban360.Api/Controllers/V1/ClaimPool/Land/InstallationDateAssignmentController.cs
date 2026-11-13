using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/installation-date-assignment")]
    public class InstallationDateAssignmentController : ControllerBase
    {
        private readonly IInstallationDateAssignmentUpdateHandler _installationDateAssignmentUpdateHandler;
        private readonly IInstallationDateAssignmentGetHandler _installationDateAssignmentGetHandler;

        public InstallationDateAssignmentController(
            IInstallationDateAssignmentUpdateHandler installationDateAssignmentUpdateHandler,
            IInstallationDateAssignmentGetHandler installationDateAssignmentGetHandler)
        {
            _installationDateAssignmentUpdateHandler = installationDateAssignmentUpdateHandler;
            _installationDateAssignmentUpdateHandler.NotNull(nameof(_installationDateAssignmentUpdateHandler));
            _installationDateAssignmentGetHandler = installationDateAssignmentGetHandler;
            _installationDateAssignmentGetHandler.NotNull(nameof(_installationDateAssignmentGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationDateAssignmentUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InstallationDateAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _installationDateAssignmentUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationDateAssignmentGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle([FromBody]SearchInput input, CancellationToken cancellationToken)
        {
            InstallationDateAssignmentGetDto installationDateAssignment = await _installationDateAssignmentGetHandler.Handle(input.Input, cancellationToken);
            return Ok(installationDateAssignment);
        }
    }
}
