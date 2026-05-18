using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Implementations;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Commands
{
    [Route("v1/water-return")]
    public class ReturnBillController : BaseController
    {
        private readonly IReturnBillPartialHandler _billPartialHandler;
        private readonly IReturnBillFullHandler _billFullHandler;
        private readonly IReturnBillByConfirmedNumberGetHandler _billByConfirmedNumberGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public ReturnBillController(
            IReturnBillPartialHandler billPartialHandler,
            IReturnBillFullHandler billFullHandler,
            IReturnBillByConfirmedNumberGetHandler billByConfirmedNumberGetHandler,
            IReportGenerator reportGenerator)
        {
            _billPartialHandler = billPartialHandler;
            _billPartialHandler.NotNull(nameof(billPartialHandler));

            _billFullHandler = billFullHandler;
            _billFullHandler.NotNull(nameof(billFullHandler));

            _billByConfirmedNumberGetHandler = billByConfirmedNumberGetHandler;
            _billByConfirmedNumberGetHandler.NotNull(nameof(billByConfirmedNumberGetHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("partial")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReturnBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PartialReturn([FromBody] ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _billPartialHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("full")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReturnBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FullReturn([FromBody] ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _billFullHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("full-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFullStiReport(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            int reportCode = 2000;
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _billFullHandler.Handle(input, CurrentUser, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("partial-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPartialStiReport(ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            int reportCode = 2000;
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _billPartialHandler.Handle(input, CurrentUser, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(ReturnBillStiInputDto input, CancellationToken cancellationToken)
        {
            int reportCode = 2000;
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _billByConfirmedNumberGetHandler.Handle(input.ConfirmedNumber, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
