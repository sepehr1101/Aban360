using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/manual-bill")]
    public class ManualBillGetController : BaseController
    {
        string _manualBillTitle = "قبوض دستی";
        private readonly IManualBillGetHandler _manualBillHandler;
        private readonly IReportGenerator _reportGenerator;
        public ManualBillGetController(
            IManualBillGetHandler manualBillHandler,
            IReportGenerator reportGenerator)
        {
            _manualBillHandler = manualBillHandler;
            _manualBillHandler.NotNull(nameof(manualBillHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(ManualBillInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto> result = await _manualBillHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }


        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ManualBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _manualBillHandler.Handle, CurrentUser, _manualBillTitle, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(ManualBillInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 700;
            ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto> manualBill = await _manualBillHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(manualBill, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
