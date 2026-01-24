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
    [Route("v1/after-sale")]
    public class AfterSaleCalcaulationManagerController : BaseController
    {
        private readonly IAfterSaleGetHandler _afterSaleGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public AfterSaleCalcaulationManagerController(
            IAfterSaleGetHandler afterSaleGetHandler,
            IReportGenerator reportGenerator)
        {
            _afterSaleGetHandler = afterSaleGetHandler;
            _afterSaleGetHandler.NotNull(nameof(afterSaleGetHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("calculate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate([FromBody] AfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto> result = await _afterSaleGetHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(AfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2010;
            FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto> data = await _afterSaleGetHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(data, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
