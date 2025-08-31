using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-modified-bills-detail")]
    public class WaterModifiedBillsDetailController : BaseController
    {
        private readonly IWaterModifiedBillsDetailHandler _modifiedBillsHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterModifiedBillsDetailController(
            IWaterModifiedBillsDetailHandler modifiedBillsHandler,
            IReportGenerator reportGenerator)
        {
            _modifiedBillsHandler = modifiedBillsHandler;
            _modifiedBillsHandler.NotNull(nameof(modifiedBillsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterModifiedBillsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto> modifiedBills = await _modifiedBillsHandler.Handle(input, cancellationToken);
            return Ok(modifiedBills);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterModifiedBillsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _modifiedBillsHandler.Handle, CurrentUser, ReportLiterals.WaterModifiedBillsDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
