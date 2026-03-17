using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class AfterSaleRequestController : BaseController
    {
        private readonly IRequestAfterSaleHandler _requestAfterSaleHandler;
        private readonly IRequestDuplicateValidation _requestDuplicateValidation;
        public AfterSaleRequestController(
            IRequestAfterSaleHandler requestAfterSaleHandler,
            IRequestDuplicateValidation requestDuplicateValidation)
        {
            _requestAfterSaleHandler = requestAfterSaleHandler;
            _requestAfterSaleHandler.NotNull(nameof(requestAfterSaleHandler));

            _requestDuplicateValidation = requestDuplicateValidation;
            _requestDuplicateValidation.NotNull(nameof(requestDuplicateValidation));

        }
        [HttpPost]
        [Route("a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestAfterSaleInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AfterSaleRequest([FromBody] RequestAfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _requestAfterSaleHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("is-duplicate/a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackingDuplicateValidationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsDuplicateAfterSaleRequest([FromBody] AfterSaleTrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingDuplicateValidationInputDto totalValidation = new(inputDto.BillId, null, null, TrackingDuplicateValidationTypeEnum.ByBillId);
            TrackingDuplicateValidationOutputDto result = await _requestDuplicateValidation.Handle(totalValidation, cancellationToken);
            return Ok(result);
        }
    }
}
