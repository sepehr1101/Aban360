using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/consumption-management-by-billid")]
    public class ConsumptionManagementByBillIdController : BaseController
    {
        private readonly IConsumptionManagementByBillIdHandler _consumptionManagerHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConsumptionManagementByBillIdController(
            IConsumptionManagementByBillIdHandler consumptionManagerHandler,
            IReportGenerator reportGenerator)
        {
            _consumptionManagerHandler = consumptionManagerHandler;
            _consumptionManagerHandler.NotNull(nameof(consumptionManagerHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConsumptionManagementByBillIdInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<MemberInfoGetDto, CosnumptionManagementByBillIdDataOutputDto> result = await _consumptionManagerHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
