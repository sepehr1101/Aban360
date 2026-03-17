using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class RequestBranchController : BaseController
    {
        private readonly IRequestNewBranchHandler _requestNewBranchHandler;
        private readonly IRequestAfterSaleHandler _requestAfterSaleHandler;
        public RequestBranchController(
            IRequestNewBranchHandler requestNewBranchHandler,
            IRequestAfterSaleHandler requestAfterSaleHandler)
        {
            _requestNewBranchHandler = requestNewBranchHandler;
            _requestNewBranchHandler.NotNull(nameof(requestNewBranchHandler));

            _requestAfterSaleHandler = requestAfterSaleHandler;
            _requestAfterSaleHandler.NotNull(nameof(requestAfterSaleHandler));
        }

        [HttpPost]
        [Route("new")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestNewBranchInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> NewRequest([FromBody] RequestNewBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = GetUserCode();
            await _requestNewBranchHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestAfterSaleInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AfterSaleRequest([FromBody] RequestAfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = GetUserCode();
            await _requestAfterSaleHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }

        private int GetUserCode()
        {
            bool isSuccess = int.TryParse(CurrentUser.Username, out int userCode);
            if (!isSuccess)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidUserName);
            }

            return userCode;
        }
    }
}
