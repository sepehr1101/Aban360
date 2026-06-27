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
    public class BillLatestListController : BaseController
    {
        private readonly IBillLatestListGetHandler _billLatestListGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public BillLatestListController(
            IBillLatestListGetHandler billLatestListGetHandler,
            IReportGenerator reportGenerator)
        {
            _billLatestListGetHandler = billLatestListGetHandler;
            _billLatestListGetHandler.NotNull(nameof(billLatestListGetHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("latest-list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] BillLatestListInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto> result = await _billLatestListGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
