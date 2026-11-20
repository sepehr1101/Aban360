using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class SetBillIdController : BaseController
    {
        private readonly IRequestFinalizationService _setBillIdHandler;
        public SetBillIdController(IRequestFinalizationService setBillIdHandler)
        {
            _setBillIdHandler = setBillIdHandler;
            _setBillIdHandler.NotNull(nameof(setBillIdHandler));
        }

        [HttpPost]
        [Route("set-billId")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchNumericInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetBillId([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            await _setBillIdHandler.SetBillIdAsync(inputDto.Input, cancellationToken);
            return Ok(inputDto);
        }
    }
}
