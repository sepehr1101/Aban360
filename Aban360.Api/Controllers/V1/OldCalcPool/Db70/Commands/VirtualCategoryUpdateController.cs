using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Commands
{
    [Route("v1/virtual-category")]
    public class VirtualCategoryUpdateController : BaseController
    {
        private readonly IVirtualCategoryUpdateHandler _virtualCategoryHandler;
        public VirtualCategoryUpdateController(IVirtualCategoryUpdateHandler virtualCategoryHandler)
        {
            _virtualCategoryHandler = virtualCategoryHandler;
            _virtualCategoryHandler.NotNull(nameof(virtualCategoryHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(VirtualCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _virtualCategoryHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
