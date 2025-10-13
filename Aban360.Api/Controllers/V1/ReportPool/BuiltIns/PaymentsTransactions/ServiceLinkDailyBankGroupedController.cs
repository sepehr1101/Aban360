using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/service-link-daily-bank-grouped")]
    public class ServiceLinkDailyBankGroupedController : BaseController
    {
        private readonly IServiceLinkDailyBankGroupedHandler _dailyBankGrouped;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkDailyBankGroupedController(
            IServiceLinkDailyBankGroupedHandler dailyBankGrouped,
            IReportGenerator reportGenerator)
        {
            _dailyBankGrouped = dailyBankGrouped;
            _dailyBankGrouped.NotNull(nameof(_dailyBankGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DailyBankGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto> dailyBankGrouped = await _dailyBankGrouped.Handle(inputDto, cancellationToken);
            return Ok(dailyBankGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, DailyBankGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _dailyBankGrouped.Handle, CurrentUser, ReportLiterals.SewageDailyBankGrouped, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(DailyBankGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 484;
            ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto> result = await _dailyBankGrouped.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
