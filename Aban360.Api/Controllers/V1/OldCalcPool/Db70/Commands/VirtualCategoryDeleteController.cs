using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Commands
{
    [Route("v1/virtual-category")]
    public class VirtualCategoryDeleteController : BaseController
    {
        private readonly IVirtualCategoryDeleteHandler _virtualCategoryHandler;
        public VirtualCategoryDeleteController(IVirtualCategoryDeleteHandler virtualCategoryHandler)
        {
            _virtualCategoryHandler = virtualCategoryHandler;
            _virtualCategoryHandler.NotNull(nameof(virtualCategoryHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategorySearchInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(VirtualCategorySearchInputDto deleteDto, CancellationToken cancellationToken)
        {
            await _virtualCategoryHandler.Handle(deleteDto, cancellationToken);
            return Ok(deleteDto);
        }
    }
}
