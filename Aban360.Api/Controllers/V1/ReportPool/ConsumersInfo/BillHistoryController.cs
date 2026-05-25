using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillHistoryController : BaseController
    {
        private readonly IBillHistoryGetHandler _billHistoryGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public BillHistoryController(
            IBillHistoryGetHandler billHistoryGetHandler,
            IReportGenerator reportGenerator)
        {
            _billHistoryGetHandler = billHistoryGetHandler;
            _billHistoryGetHandler.NotNull(nameof(billHistoryGetHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("history-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillHistoryHeaderOutputDto, BillHistoryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] BillHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillHistoryHeaderOutputDto, BillHistoryDataOutputDto> result = await _billHistoryGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("history-excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] BillHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            //await _reportGenerator.FireAndInform(inputDto, cancellationToken, _billHistoryGetHandler.Handle, CurrentUser, ReportLiterals.BillsHistory, connectionId);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("history-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSti([FromBody] BillHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2090;
            ReportOutput<BillHistoryHeaderOutputDto, BillHistoryDataOutputDto> result = await _billHistoryGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
