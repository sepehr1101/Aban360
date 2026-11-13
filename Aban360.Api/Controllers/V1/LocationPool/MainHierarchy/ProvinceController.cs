using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/province")]
    public class ProvinceController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceService _service;

        public ProvinceController(IUnitOfWork uow, IProvinceService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProvinceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProvinceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ProvinceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ProvinceGetDto result =await _service.GetSingleAsync(id,cancellationToken);
            return Ok(result);
        }

        [HttpGet,HttpPost]
        [Route("All")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ProvinceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ProvinceGetDto> result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
