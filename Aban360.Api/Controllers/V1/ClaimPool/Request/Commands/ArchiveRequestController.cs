using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
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
        private readonly IArchivedRequestHandler _archivedRequestHandler;
        public ArchiveRequestController(
            IArchiveRequestHandler archiveRequestHandler,
            IArchivedRequestHandler archivedRequestHandler)
        {
            _archiveRequestHandler = archiveRequestHandler;
            _archiveRequestHandler.NotNull(nameof(archiveRequestHandler));

            _archivedRequestHandler = archivedRequestHandler;
            _archivedRequestHandler.NotNull(nameof(archivedRequestHandler));
        }

        [HttpGet,HttpPost]
        [Route("archived")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Archived(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto> result = await _archivedRequestHandler.Handle(cancellationToken);
            return Ok(result);
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
