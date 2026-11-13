using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionService _service;

        public CordinalDirectionController(IUnitOfWork uow, ICordinalDirectionService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CordinalDirectionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CordinalDirectionUpdateDto updatedto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updatedto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updatedto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("Single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSigle(short id, CancellationToken cancellationToken)
        {
            CordinalDirectionGetDto cordinalDirection = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(cordinalDirection);
        }

        [HttpGet, HttpPost]
        [Route("All")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CordinalDirectionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
        {
            ICollection<CordinalDirectionGetDto> cordinalDirection = await _service.GetAllAsync(cancellationToken);
            return Ok(cordinalDirection);
        }
    }
}
