using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class ArchiveRequestController : BaseController
    {
        private readonly IArchiveRequestHandler _archiveRequestHandler;
        public ArchiveRequestController(IArchiveRequestHandler archiveRequestHandler)
        {
            _archiveRequestHandler = archiveRequestHandler;
            _archiveRequestHandler.NotNull(nameof(archiveRequestHandler));
        }

        [HttpPost]
        [Route("archive")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ArchiveRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Archive([FromBody] ArchiveRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _archiveRequestHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }
    }
}
