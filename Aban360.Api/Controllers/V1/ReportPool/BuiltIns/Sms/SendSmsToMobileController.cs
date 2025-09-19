using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.Sms
{
    [Route("v1/send-sms-to-mobile")]
    public class SendSmsToMobileController : BaseController
    {
        private readonly ISendSmsToMobileHandler _sendSmsToMobileHandler;
        private readonly IReportGenerator _reportGenerator;
        public SendSmsToMobileController(
            ISendSmsToMobileHandler sendSmsToMobileHandler,
            IReportGenerator reportGenerator)
        {
            _sendSmsToMobileHandler = sendSmsToMobileHandler;
            _sendSmsToMobileHandler.NotNull(nameof(sendSmsToMobileHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SendSmsToMobileInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto> SendSmsToMobile = await _sendSmsToMobileHandler.Handle(input, cancellationToken);
            return Ok(SendSmsToMobile);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SendSmsToMobileInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sendSmsToMobileHandler.Handle, CurrentUser, ReportLiterals.SendSmsToMobile, connectionId);
            return Ok(inputDto);
        }
        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SendSmsToMobileInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 290;
            ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto> result = await _sendSmsToMobileHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

    }
}
