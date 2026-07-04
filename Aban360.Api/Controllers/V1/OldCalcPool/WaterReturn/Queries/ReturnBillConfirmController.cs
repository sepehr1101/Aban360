using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Queries
{
    [Route("v1/water-return")]
    public class ReturnBillConfirmController : BaseController
    {
        private readonly IUnconfirmedBillReturnGetByZoneHandler _unconfirmedBillReturnGetByZoneHandler;
        private readonly IUnconfirmedBillReturnGetByBillIdHandler _unconfirmedBillReturnGetByBillIdHandler;
        public ReturnBillConfirmController(
            IUnconfirmedBillReturnGetByZoneHandler unconfirmedBillReturnGetByZoneHandler,
            IUnconfirmedBillReturnGetByBillIdHandler unconfirmedBillReturnGetByBillIdHandler)
        {
            _unconfirmedBillReturnGetByZoneHandler = unconfirmedBillReturnGetByZoneHandler;
            _unconfirmedBillReturnGetByZoneHandler.NotNull(nameof(unconfirmedBillReturnGetByZoneHandler));

            _unconfirmedBillReturnGetByBillIdHandler = unconfirmedBillReturnGetByBillIdHandler;
            _unconfirmedBillReturnGetByBillIdHandler.NotNull(nameof(unconfirmedBillReturnGetByBillIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("unconfirmed/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnConfirmed(int zoneId, CancellationToken cancellationToken)
        {
            ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto> result = await _unconfirmedBillReturnGetByZoneHandler.Handle(zoneId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("unconfirmed-billId/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnConfirmedByBillId(string billId, CancellationToken cancellationToken)
        {
            ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto> result = await _unconfirmedBillReturnGetByBillIdHandler.Handle(billId, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
