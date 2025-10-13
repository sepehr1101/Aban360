using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/without-bill")]
    public class WithoutBillController : BaseController
    {
        private readonly IWithoutBillHandler _withoutBillHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutBillController(
            IWithoutBillHandler withoutBillHandler,
            IReportGenerator reportGenerator)
        {
            _withoutBillHandler = withoutBillHandler;
            _withoutBillHandler.NotNull(nameof(withoutBillHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto> withoutBill = await _withoutBillHandler.Handle(input, cancellationToken);
            return Ok(withoutBill);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutBillHandler.Handle, CurrentUser, ReportLiterals.WithoutBill, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WithoutBillInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 210;
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto> nonPermanentBranch = await _withoutBillHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(nonPermanentBranch, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
