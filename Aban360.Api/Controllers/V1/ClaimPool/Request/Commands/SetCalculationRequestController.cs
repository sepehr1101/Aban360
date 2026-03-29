using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class SetCalculationRequestController : BaseController
    {
        private readonly ISetCalculationRequestHandler _setCalculationRequestHandler;
        public SetCalculationRequestController(ISetCalculationRequestHandler setCalculationRequestHandler)
        {
            _setCalculationRequestHandler = setCalculationRequestHandler;
            _setCalculationRequestHandler.NotNull(nameof(setCalculationRequestHandler));
        }

        [HttpPost]
        [Route("calculation-set")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetCalculationRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculationSet([FromBody] SetCalculationRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _setCalculationRequestHandler.Handle(inputDto,userCode, cancellationToken);
            return Ok(inputDto);
        }
    }
}