using Aban360.OldCalcPool.Application.Features.Db70.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70
{
    [Route("v1/virtual-category")]
    public class VirtualCategoryController : BaseController
    {
        private readonly IVirtualCategoryService _service;
        public VirtualCategoryController(IVirtualCategoryService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchShortInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(SearchShortInputDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(VirtualCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            IEnumerable<VirtualCategoryGetDto> result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<VirtualCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(SearchShortInputDto inputDto, CancellationToken cancellationToken)
        {
            VirtualCategoryGetDto result=await _service.GetSingleAsync(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
