using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/tanker")]
    public class TankerGetController : BaseController
    {
        private readonly ITankerWaterGetHandler _getHandler;
        private readonly IReportGenerator _reportGenerator;
        public TankerGetController(
            ITankerWaterGetHandler getHandler,
            IReportGenerator reportGenerator)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("get-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] TankerWaterInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto> result = await _getHandler.Handle(inputDto, cancellationToken);

            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get-excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] TankerWaterInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _getHandler.Handle, CurrentUser, ReportLiterals.Tanker, connectionId);
            return Ok(inputDto);
        }
    }
}
