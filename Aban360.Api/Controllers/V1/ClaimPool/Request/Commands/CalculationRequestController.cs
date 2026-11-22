using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;
using Aban360.CalculationPool.Application.Features.Sale.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class CalculationRequestController : BaseController
    {
        private readonly ICalculationRequestPricingService _calculationRequestHandler;
        private readonly ICalculationRequestWorkflowService _calculationRequestWorkflowService;
        private readonly IManualCalculationRequestService _manualCalculationRequestService;
        public CalculationRequestController(ICalculationRequestPricingService calculationRequestHandler, ICalculationRequestWorkflowService calculationRequestWorkflowService, IManualCalculationRequestService manualCalculationRequestService)
        {
            _calculationRequestHandler = calculationRequestHandler;
            _calculationRequestHandler.NotNull(nameof(calculationRequestHandler));
            _calculationRequestWorkflowService = calculationRequestWorkflowService;
            _calculationRequestWorkflowService.NotNull(nameof(calculationRequestWorkflowService));
            _manualCalculationRequestService = manualCalculationRequestService;
            _manualCalculationRequestService.NotNull(nameof(manualCalculationRequestService));
        }

        [HttpPost]
        [Route("calculation")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate([FromBody] TrackNumberWithDescriptionInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await _calculationRequestHandler.CalculationRequestAsync(inputDto.TrackNumber, userCode, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("calculation/{trackNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculateByTrackNumber(int trackNumber, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await _calculationRequestHandler.CalculationRequestAsync(trackNumber, userCode, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("reCalculate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberWithDescriptionInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReCalculate([FromBody] TrackNumberWithDescriptionInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _calculationRequestWorkflowService.ReCalculationRequestAsync(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("calculation-insert-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SaleAndAfterSaleDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertManualCalculation([FromBody] KartInsertManualInputDto inputDto, CancellationToken cancellationToken)
        {
            SaleAndAfterSaleDataOutputDto result = await _manualCalculationRequestService.InsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("calculation-remove-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<KartRemoveManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveManualCalculation([FromBody] KartRemoveManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _manualCalculationRequestService.RemoveAsync(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("calculation-display")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculationDisplay([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await _calculationRequestWorkflowService.CalculationRequestDisplayAsync(inputDto.Input, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("calculation-display/{trackNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculationDisplayByTrackNumber(int trackNumber, CancellationToken cancellationToken)
        {
            ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await _calculationRequestWorkflowService.CalculationRequestDisplayAsync(trackNumber, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("calculation-confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetCalculationRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculationConfirm([FromBody] SetCalculationRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _calculationRequestWorkflowService.CalculationRequestConfirmAsync(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }
    }
}
