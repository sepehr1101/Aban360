using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/bill-installment")]
    public class BillInstallmentController : BaseController
    {
        private readonly IBillInstallmentCreateHandler _billInstallmentCreateHandler;
        private readonly IBillInstallmentGetHandler _billInstallmentGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public BillInstallmentController(
            IBillInstallmentCreateHandler billInstallmentCreateHandler,
            IBillInstallmentGetHandler billInstallmentGetHandler,
            IReportGenerator reportGenerator)
        {
            _billInstallmentCreateHandler = billInstallmentCreateHandler;
            _billInstallmentCreateHandler.NotNull(nameof(billInstallmentCreateHandler));

            _billInstallmentGetHandler = billInstallmentGetHandler;
            _billInstallmentGetHandler.NotNull(nameof(billInstallmentGetHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddInstallment([FromBody] BillInstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> result = await _billInstallmentCreateHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto> result = await _billInstallmentGetHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(SearchInput inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2020;
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto> result = await _billInstallmentGetHandler.Handle(inputDto.Input, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
