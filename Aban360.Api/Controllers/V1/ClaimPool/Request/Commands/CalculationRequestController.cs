using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
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
        private readonly ICalculationRequestDisplayHandler _calculationRequestDisplayHandler;
        public CalculationRequestController(
            ICalculationRequestHandler calculationRequestHandler,
            ICalculationRequestInsertManualHandler calculationRequestInsertManualHandler,
            ICalculationRequestRemoveManualHandler calculationRequestRemoveManualHandler,
            ICalculationRequestDisplayHandler calculationRequestDisplayHandler)
        {
            _calculationRequestHandler = calculationRequestHandler;
            _calculationRequestHandler.NotNull(nameof(calculationRequestHandler));

            _calculationRequestInsertManualHandler = calculationRequestInsertManualHandler;
            _calculationRequestInsertManualHandler.NotNull(nameof(calculationRequestInsertManualHandler));

            _calculationRequestRemoveManualHandler = calculationRequestRemoveManualHandler;
            _calculationRequestRemoveManualHandler.NotNull(nameof(calculationRequestRemoveManualHandler));

            _calculationRequestDisplayHandler = calculationRequestDisplayHandler;
            _calculationRequestDisplayHandler.NotNull(nameof(calculationRequestDisplayHandler));
        }


        [HttpPost]
        [Route("calculation")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await _calculationRequestHandler.Handle(inputDto.Input, userCode, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("calculation-insert-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<KartInsertManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertManualCalculation([FromBody] KartInsertManualInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _calculationRequestInsertManualHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("calculation-remove-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<KartRemoveManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveManualCalculation([FromBody] KartRemoveManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _calculationRequestRemoveManualHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("calculation-display")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CalculationRequestDisplayHeaderOutputDto, CalculationRequestDisplayDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculationDisplay([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<CalculationRequestDisplayHeaderOutputDto, CalculationRequestDisplayDataOutputDto> result = await _calculationRequestDisplayHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(result);
        }

    }
}