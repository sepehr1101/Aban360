using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class ArchiveRequestController : BaseController
    {
        private readonly IRequestLifecycleService _requestLifecycleService;
        public ArchiveRequestController(IRequestLifecycleService requestLifecycleService)
        {
            _requestLifecycleService = requestLifecycleService;
            _requestLifecycleService.NotNull(nameof(requestLifecycleService));
        }

        [HttpGet, HttpPost]
        [Route("archived")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Archived(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto> result = await _requestLifecycleService.ArchivedRequestAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("archive")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ArchiveRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Archive([FromBody] ArchiveRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _requestLifecycleService.ArchiveRequestAsync(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }
    }
}
