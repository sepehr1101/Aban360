using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class SetAmountConfirmRequestController : BaseController
    {
        private readonly IAmountRequestConfirmHandler _setamountConfirmRequestHandler;
        public SetAmountConfirmRequestController(IAmountRequestConfirmHandler setamountConfirmRequestHandler)
        {
            _setamountConfirmRequestHandler = setamountConfirmRequestHandler;
            _setamountConfirmRequestHandler.NotNull(nameof(setamountConfirmRequestHandler));
        }

        [HttpPost]
        [Route("amount-confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetCalculationRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AmountConfirm([FromBody] SetCalculationRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _setamountConfirmRequestHandler.Handle(inputDto,userCode, cancellationToken);
            return Ok(inputDto);
        }
    }
}