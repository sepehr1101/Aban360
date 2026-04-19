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
    public class NewRequestController : BaseController
    {
        private readonly IRequestNewBranchHandler _requestNewBranchHandler;
        private readonly IRequestDuplicateValidationHandler _requestDuplicateValidation;
        public NewRequestController(
            IRequestNewBranchHandler requestNewBranchHandler,
            IRequestDuplicateValidationHandler requestDuplicateValidation)
        {
            _requestNewBranchHandler = requestNewBranchHandler;
            _requestNewBranchHandler.NotNull(nameof(requestNewBranchHandler));

            _requestDuplicateValidation = requestDuplicateValidation;
            _requestDuplicateValidation.NotNull(nameof(requestDuplicateValidation));
        }

        [HttpPost]
        [Route("new")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestNewBranchInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> NewRequest([FromBody] RequestNewBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _requestNewBranchHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("is-duplicate/new")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackingDuplicateValidationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsDuplicateNewRequest([FromBody] NewTrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingDuplicateValidationInputDto totalValidation = new(null, inputDto.NeighbourBillId, inputDto.NationalCode, TrackingDuplicateValidationTypeEnum.ByNationalCode);
            TrackingDuplicateValidationOutputDto result = await _requestDuplicateValidation.Handle(totalValidation, cancellationToken);
            return Ok(result);
        }
    }
}
