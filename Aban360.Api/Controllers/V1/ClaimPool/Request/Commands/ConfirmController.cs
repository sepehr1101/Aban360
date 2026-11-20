using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class ConfirmController : BaseController
    {
        private readonly IRequestFinalizationService _confirmHandler;
        public ConfirmController(IRequestFinalizationService confirmHandler)
        {
            _confirmHandler = confirmHandler;
            _confirmHandler.NotNull(nameof(confirmHandler));
        }

        [HttpPost]
        [Route("confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestConfirmInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetConfirm([FromBody] RequestConfirmInputDto inputDto, CancellationToken cancellationToken)
        {
            await _confirmHandler.RequestCofirmeAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
