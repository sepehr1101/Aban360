using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class CalculationRequestController : BaseController
    {
        private readonly ICalculationRequestHandler _calculationRequestHandler;
        private readonly ICalculationRequestInsertManualHandler _calculationRequestInsertManualHandler;
        private readonly ICalculationRequestRemoveManualHandler _calculationRequestRemoveManualHandler;
        public CalculationRequestController(
            ICalculationRequestHandler calculationRequestHandler,
            ICalculationRequestInsertManualHandler calculationRequestInsertManualHandler,
            ICalculationRequestRemoveManualHandler calculationRequestRemoveManualHandler)
        {
            _calculationRequestHandler = calculationRequestHandler;
            _calculationRequestHandler.NotNull(nameof(calculationRequestHandler));

            _calculationRequestInsertManualHandler = calculationRequestInsertManualHandler;
            _calculationRequestInsertManualHandler.NotNull(nameof(calculationRequestInsertManualHandler));

            _calculationRequestRemoveManualHandler = calculationRequestRemoveManualHandler;
            _calculationRequestRemoveManualHandler.NotNull(nameof(calculationRequestRemoveManualHandler));
        }


        [HttpPost]
        [Route("calculation")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation(SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await _calculationRequestHandler.Handle(inputDto.Input, userCode, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("calculation-insert-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertManualCalculation(KartInsertManualInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _calculationRequestInsertManualHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("calculation-remove-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveManualCalculation(KartRemoveManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _calculationRequestRemoveManualHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }

    }
}