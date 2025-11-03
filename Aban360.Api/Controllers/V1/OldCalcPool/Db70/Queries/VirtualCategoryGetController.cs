using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Queries
{
    [Route("v1/virtual-category")]
    public class VirtualCategoryGetController : BaseController
    {
        private readonly IVirtualCategoryGetHandler _virtualCategoryHandler;
        public VirtualCategoryGetController(IVirtualCategoryGetHandler virtualCategoryHandler)
        {
            _virtualCategoryHandler = virtualCategoryHandler;
            _virtualCategoryHandler.NotNull(nameof(virtualCategoryHandler));
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(VirtualCategorySearchInputDto inputDto, CancellationToken cancellationToken)
        {
            VirtualCategoryGetDto result=await _virtualCategoryHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
