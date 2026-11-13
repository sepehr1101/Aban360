using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70
{
    [Route("v1/virtual-category")]
    public class VirtualCategoryController : BaseController
    {
        private readonly IVirtualCategoryCreateHandler _virtualCategoryCreateHandler;
        private readonly IVirtualCategoryDeleteHandler _virtualCategoryDeleteHandler;
        private readonly IVirtualCategoryUpdateHandler _virtualCategoryUpdateHandler;
        private readonly IVirtualCategoryGetAllHandler _virtualCategoryGetAllHandler;
        private readonly IVirtualCategoryGetHandler _virtualCategoryGetHandler;

        public VirtualCategoryController(
            IVirtualCategoryCreateHandler virtualCategoryCreateHandler,
            IVirtualCategoryDeleteHandler virtualCategoryDeleteHandler,
            IVirtualCategoryUpdateHandler virtualCategoryUpdateHandler,
            IVirtualCategoryGetAllHandler virtualCategoryGetAllHandler,
            IVirtualCategoryGetHandler virtualCategoryGetHandler)
        {
            _virtualCategoryCreateHandler = virtualCategoryCreateHandler;
            _virtualCategoryCreateHandler.NotNull(nameof(virtualCategoryCreateHandler));
            _virtualCategoryDeleteHandler = virtualCategoryDeleteHandler;
            _virtualCategoryDeleteHandler.NotNull(nameof(virtualCategoryDeleteHandler));
            _virtualCategoryUpdateHandler = virtualCategoryUpdateHandler;
            _virtualCategoryUpdateHandler.NotNull(nameof(virtualCategoryUpdateHandler));
            _virtualCategoryGetAllHandler = virtualCategoryGetAllHandler;
            _virtualCategoryGetAllHandler.NotNull(nameof(virtualCategoryGetAllHandler));
            _virtualCategoryGetHandler = virtualCategoryGetHandler;
            _virtualCategoryGetHandler.NotNull(nameof(virtualCategoryGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _virtualCategoryCreateHandler.Handle(createDto, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchShortInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(SearchShortInputDto deleteDto, CancellationToken cancellationToken)
        {
            await _virtualCategoryDeleteHandler.Handle(deleteDto, cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(VirtualCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _virtualCategoryUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            IEnumerable<VirtualCategoryGetDto> result = await _virtualCategoryGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(SearchShortInputDto inputDto, CancellationToken cancellationToken)
        {
            VirtualCategoryGetDto result=await _virtualCategoryGetHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
