using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Commands
{
    [Route("v1/virtual-category")]
    public class VirtualCategoryCreateController : BaseController
    {
        private readonly IVirtualCategoryCreateHandler _virtualCategoryHandler;
        public VirtualCategoryCreateController(IVirtualCategoryCreateHandler virtualCategoryHandler)
        {
            _virtualCategoryHandler = virtualCategoryHandler;
            _virtualCategoryHandler.NotNull(nameof(virtualCategoryHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _virtualCategoryHandler.Handle(createDto, cancellationToken);
            return Ok(createDto);
        }
    }
}
