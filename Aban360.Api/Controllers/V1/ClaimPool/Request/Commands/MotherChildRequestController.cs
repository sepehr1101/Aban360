using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;

using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class MotherChildRequestController : BaseController
    {
        private readonly IMotherChildRequestService _motherChildRequestService;
        public MotherChildRequestController(IMotherChildRequestService motherChildRequestService)
        {
            _motherChildRequestService = motherChildRequestService;
            _motherChildRequestService.NotNull(nameof(motherChildRequestService));
        }

        [HttpPost]
        [Route("set-common-siphon")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CommonSiphonRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetCommonSiphon([FromBody] CommonSiphonRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _motherChildRequestService.SetCommonSiphonAsync(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-common-siphon")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CommonSiphonUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCommonSiphon([FromBody] CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _motherChildRequestService.UpdateAsync(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("set-mother-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetMotherChild([FromBody] MotherChildRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _motherChildRequestService.SetMotherChildAsync(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-mother-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMotherChild([FromBody] MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _motherChildRequestService.UpdateAsync(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("delete-mother-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMotherChild([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            await _motherChildRequestService.DeleteAsync(inputDto.Input, cancellationToken);
            return Ok(inputDto);
        }
    }
}
