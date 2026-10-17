using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Queries
{
    [Route("v1/water-return")]
    public class WaterReturnedController : BaseController
    {
        private readonly IReturedBillAfterDateLockGetHandler _returnedAfterDateLockHandler;
        private readonly IReturnedBillRemoveHandler _returnedBillRemoveHandler;
        public WaterReturnedController(
            IReturedBillAfterDateLockGetHandler returnedAfterDateLockHandler,
            IReturnedBillRemoveHandler returnedBillRemoveHandler)
        {
            _returnedAfterDateLockHandler = returnedAfterDateLockHandler;
            _returnedAfterDateLockHandler.NotNull(nameof(returnedAfterDateLockHandler));

            _returnedBillRemoveHandler = returnedBillRemoveHandler;
            _returnedBillRemoveHandler.NotNull(nameof(returnedBillRemoveHandler));
        }

        [HttpPost]
        [Route("confirmed")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ReturnedBillsOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfirmed([FromBody] ZoneIdWrapper zoneIdWrapper, CancellationToken cancellationToken)
        {
            IEnumerable<ReturnedBillsOutputDto> result = await _returnedAfterDateLockHandler.Handle(zoneIdWrapper.ZoneId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<WaterReturedRemoveInputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveReturned(WaterReturedRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _returnedBillRemoveHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
