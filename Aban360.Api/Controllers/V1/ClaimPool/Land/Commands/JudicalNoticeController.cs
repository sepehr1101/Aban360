using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/judical-notice")]
    public class JudicalNoticeController : BaseController
    {
        private readonly IJudicalNoticeCommandHandler _judicalNoticeCommandHandler;
        private readonly IJudicialNoticeSetResultHandler _judicialNoticeSetResultHandler;
        private readonly ISmsOldHandler _smsHandler;
        private readonly IBackgroundJobClient _jobClient;
        public JudicalNoticeController(
            IJudicalNoticeCommandHandler judicalNoticeCommandHandler,
            IJudicialNoticeSetResultHandler judicialNoticeSetResultHandler,
            ISmsOldHandler smsHandler,
            IBackgroundJobClient jobClient)
        {
            _judicalNoticeCommandHandler = judicalNoticeCommandHandler;
            _judicalNoticeCommandHandler.NotNull(nameof(judicalNoticeCommandHandler));

            _judicialNoticeSetResultHandler = judicialNoticeSetResultHandler;
            _judicialNoticeSetResultHandler.NotNull(nameof(smsHandler));

            _smsHandler = smsHandler;
            _smsHandler.NotNull(nameof(smsHandler));

            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));
        }

        [HttpPost, HttpGet]
        [Route("command")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommandSti([FromBody] JudicalNoticeCommandInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2120;
            FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto> result = await _judicalNoticeCommandHandler.Handle(inputDto, CurrentUser, cancellationToken);
            _jobClient.Enqueue(() => _smsHandler.Send("09135742556", result.ReportHeader.Message, Guid.NewGuid()));

            JsonReportId jsonReport = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode, true);
            return Ok(jsonReport);
        }

        [HttpPost, HttpGet]
        [Route("set-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JudicalNoticeSetResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetResult([FromBody] JudicalNoticeSetResultInputDto inputDto, CancellationToken cancellationToken)
        {
            await _judicialNoticeSetResultHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetResults(CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> results = _judicialNoticeSetResultHandler.GetJudicialResults();
            return Ok(results);
        }
    }
}
