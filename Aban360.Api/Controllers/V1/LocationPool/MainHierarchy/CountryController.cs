using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/country")]
    public class CountryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICountryService _service;

        public CountryController(IUnitOfWork uow, ICountryService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CountryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CountryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CountryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id,CancellationToken cancellationToken)
        {
            CountryGetDto result = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CountryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CountryGetDto> result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

    }
}
