using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Microsoft.AspNetCore.Mvc;
using ITankerWaterQueriesService = global::Aban360.CalculationPool.Application.Features.Sale.Services.ITankerWaterQueriesService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/tanker")]
    public class TankerGetController : BaseController
    {
        private readonly ITankerWaterQueriesService _tankerWaterQueriesService;
        private readonly IReportGenerator _reportGenerator;
        public TankerGetController(ITankerWaterQueriesService tankerWaterQueriesService, IReportGenerator reportGenerator)
        {
            _tankerWaterQueriesService = tankerWaterQueriesService;
            _tankerWaterQueriesService.NotNull(nameof(tankerWaterQueriesService));
            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("get-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] TankerWaterInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto> result = await _tankerWaterQueriesService.TankerWaterGetAsync(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get-excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] TankerWaterInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _tankerWaterQueriesService.TankerWaterGetAsync, CurrentUser, ReportLiterals.Tanker, connectionId);
            return Ok(inputDto);
        }
    }
}
