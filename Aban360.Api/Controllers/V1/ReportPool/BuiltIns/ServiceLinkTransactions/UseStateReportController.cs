using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/use-state-report")]
    public class UseStateReportController:BaseController
    {
        private readonly IUseStateReportHandler _useStateReportHandler;
        private readonly IReportGenerator _reportGenerator;
        public UseStateReportController(
            IUseStateReportHandler useStateReportHandler,
            IReportGenerator reportGenerator)
        {
            _useStateReportHandler = useStateReportHandler;
            _useStateReportHandler.NotNull(nameof(_useStateReportHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>>),StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRaw(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto> useStates =await _useStateReportHandler.Handle(inputDto,cancellationToken);
            return Ok(useStates);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _useStateReportHandler.Handle, CurrentUser, ReportLiterals.UseStateReport, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 60;
            ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto> useStates = await _useStateReportHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(useStates, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
