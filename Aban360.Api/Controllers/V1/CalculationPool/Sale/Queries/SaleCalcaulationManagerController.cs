using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/sale")]
    public class SaleCalcaulationManagerController : BaseController
    {
        private readonly ISaleGetHandler _getHandler;
        private readonly IReportGenerator _reportGenerator;
        public SaleCalcaulationManagerController(
            ISaleGetHandler getHandler,
            IReportGenerator reportGenerator)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("calculate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate([FromBody] SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> result = await _getHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2011;
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> data = await _getHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(data, cancellationToken, reportCode);
            return Ok(reportId);
        }

    }
}
