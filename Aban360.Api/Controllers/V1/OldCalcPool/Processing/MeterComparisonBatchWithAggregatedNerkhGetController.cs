using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/meter-comparison-batch-with-aggregated-nerkh")]
    public class MeterComparisonBatchWithAggregatedNerkhGetController : BaseController
    {
        private readonly IMeterComparisonBatchWithAggregatedNerkhGetHandler _meterComparisonBatchWithAggregatedNerkhGet;
        private readonly IReportGenerator _reportGenerator;
        public MeterComparisonBatchWithAggregatedNerkhGetController(
            IMeterComparisonBatchWithAggregatedNerkhGetHandler meterComparisonBatchWithAggregatedNerkhGet,
            IReportGenerator reportGenerator)
        {
            _meterComparisonBatchWithAggregatedNerkhGet = meterComparisonBatchWithAggregatedNerkhGet;
            _meterComparisonBatchWithAggregatedNerkhGet.NotNull(nameof(_meterComparisonBatchWithAggregatedNerkhGet));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MeterComparisonBatchInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto> meterComparisonBatchGet = await _meterComparisonBatchWithAggregatedNerkhGet.Handle(inputDto, cancellationToken);
            return Ok(meterComparisonBatchGet);
        }


        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MeterComparisonBatchInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterComparisonBatchWithAggregatedNerkhGet.Handle, CurrentUser, "مغایرت تعرفه", connectionId);
            return Ok(inputDto);
        }
    }
}
