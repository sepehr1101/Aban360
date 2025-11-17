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
    [Route("v1/meter-life")]
    public class MeterLifeDetailController : BaseController
    {
        private readonly IMeterLifeDetailHandler _meterLifeHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterLifeDetailController(
            IMeterLifeDetailHandler meterLifeHandler,
            IReportGenerator reportGenerator)
        {
            _meterLifeHandler = meterLifeHandler;
            _meterLifeHandler.NotNull(nameof(meterLifeHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet,HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(MeterLifeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto> result = await _meterLifeHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MeterLifeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterLifeHandler.Handle, CurrentUser, ReportLiterals.MeterLifeDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(MeterLifeInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 690;
            ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto> result = await _meterLifeHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
