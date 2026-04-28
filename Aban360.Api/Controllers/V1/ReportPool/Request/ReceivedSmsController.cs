using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Request
{
    [Route("v1/sms")]
    public class ReceivedSmsController : BaseController
    {
        private readonly IReceivedSmdByPaigingHandler _receivedSmdByPaigingHandler;
        public ReceivedSmsController(IReceivedSmdByPaigingHandler receivedSmdByPaigingHandler)
        {
            _receivedSmdByPaigingHandler = receivedSmdByPaigingHandler;
            _receivedSmdByPaigingHandler.NotNull(nameof(receivedSmdByPaigingHandler));
        }

        [HttpPost]
        [Route("received-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivedSmsByPaiginationRaw([FromBody] ReceivedSmsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto> result = await _receivedSmdByPaigingHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("received-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivedSmsByPaiginationSti([FromBody] ReceivedSmsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2060;
            ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto> result = await _receivedSmdByPaigingHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
