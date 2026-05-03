using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/tanker-water")]
    public class TankerWaterController : BaseController
    {
        private readonly ITankerInsertHandler _tankerInserHandler;
        private readonly ITankerRemoveHandler _tankerRemoveHandler;
        public TankerWaterController(
            ITankerRemoveHandler tankerRemoveHandler,
            ITankerInsertHandler tankerInserHandler)
        {
            _tankerRemoveHandler = tankerRemoveHandler;
            _tankerRemoveHandler.NotNull(nameof(tankerRemoveHandler));

            _tankerInserHandler = tankerInserHandler;
            _tankerInserHandler.NotNull(nameof(tankerInserHandler));
        }

        [HttpGet, HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] TankerInsertInputDto input, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            TankerWaterCalculationOutputDto result = await _tankerInserHandler.Handle(input, userCode, cancellationToken);
            return Ok(result);
        }


        [HttpGet, HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] TankerRemoveInputDto input, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _tankerRemoveHandler.Handle(input, userCode, cancellationToken);
            return Ok(input);
        }
    }
}
