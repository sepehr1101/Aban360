using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/bill-installment")]
    public class BillsInstallmentController : BaseController
    {
        private readonly IBillInstallmentCreateHandler _billInstallmentCreateHandler;
        private readonly IReportGenerator _reportGenerator;
        public BillsInstallmentController(
            IBillInstallmentCreateHandler billInstallmentCreateHandler,
            IReportGenerator reportGenerator)
        {
            _billInstallmentCreateHandler = billInstallmentCreateHandler;
            _billInstallmentCreateHandler.NotNull(nameof(billInstallmentCreateHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCanRemoved([FromBody] GhestAbInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> result = await _billInstallmentCreateHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(GhestAbInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2020;
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> result = await _billInstallmentCreateHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
