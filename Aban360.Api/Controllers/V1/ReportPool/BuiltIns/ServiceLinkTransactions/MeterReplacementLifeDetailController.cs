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
    [Route("v1/meter-replacement-life-detail")]
    public class MeterReplacementLifeDetailController : BaseController
    {
        private readonly IMeterReplacementLifeDetailHandler _meterReplacementLifeHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterReplacementLifeDetailController(
            IMeterReplacementLifeDetailHandler meterReplacementLifeHandler,
            IReportGenerator reportGenerator)
        {
            _meterReplacementLifeHandler = meterReplacementLifeHandler;
            _meterReplacementLifeHandler.NotNull(nameof(meterReplacementLifeHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(MeterReplacementLifeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto> result = await _meterReplacementLifeHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MeterReplacementLifeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterReplacementLifeHandler.Handle, CurrentUser, ReportLiterals.MeterReplacementLifeDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(MeterReplacementLifeInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 730;
            ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto> result = await _meterReplacementLifeHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
