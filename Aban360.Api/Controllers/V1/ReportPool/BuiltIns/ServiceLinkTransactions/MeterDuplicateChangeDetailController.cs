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
    [Route("v1/meter-duplicate-change-detail")]
    public class MeterDuplicateChangeDetailController : BaseController
    {
        private readonly IMeterDuplicateChangeDetailHandler _meterDuplicateChangeDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterDuplicateChangeDetailController(
            IMeterDuplicateChangeDetailHandler meterDuplicateChangeDetailHandler,
            IReportGenerator reportGenerator)
        {
            _meterDuplicateChangeDetailHandler = meterDuplicateChangeDetailHandler;
            _meterDuplicateChangeDetailHandler.NotNull(nameof(meterDuplicateChangeDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Raw(MeterDuplicateChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto> result = await _meterDuplicateChangeDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> Excel(string connectionId, MeterDuplicateChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterDuplicateChangeDetailHandler.Handle, CurrentUser, ReportLiterals.MeterDuplicateChangeDetail, connectionId);
            return Ok(inputDto);
        }   

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(MeterDuplicateChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 720;
            ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto> result = await _meterDuplicateChangeDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
