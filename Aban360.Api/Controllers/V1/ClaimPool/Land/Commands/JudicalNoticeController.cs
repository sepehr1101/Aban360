using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/judical-notice")]
    public class JudicalNoticeController : BaseController
    {
        private readonly IJudicalNoticeCommandHandler _judicalNoticeCommandHandler;
        public JudicalNoticeController(IJudicalNoticeCommandHandler judicalNoticeCommandHandler)
        {
            _judicalNoticeCommandHandler = judicalNoticeCommandHandler;
            _judicalNoticeCommandHandler.NotNull(nameof(judicalNoticeCommandHandler));
        }

        [HttpPost, HttpGet]
        [Route("command")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommandSti([FromBody] JudicalNoticeCommandInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2120;
            FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto> result = await _judicalNoticeCommandHandler.Handle(inputDto, CurrentUser, cancellationToken);
            JsonReportId jsonReport = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode, true);
            return Ok(jsonReport);
        }
    }
}
