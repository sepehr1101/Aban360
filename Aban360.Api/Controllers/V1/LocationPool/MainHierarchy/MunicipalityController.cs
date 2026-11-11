using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/municipality")]
    public class MunicipalityController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMunicipalityCreateHandler _municipalityCreateHandler;
        private readonly IMunicipalityUpdateHandler _municipalityUpdateHandler;
        private readonly IMunicipalityDeleteHandler _municipalityDeleteHandler;
        private readonly IMunicipalityGetSingleHandler _municipalityGetSingleHandler;
        private readonly IMunicipalityGetAllHandler _municipalityGetAllHandler;

        public MunicipalityController(
            IUnitOfWork uow,
            IMunicipalityCreateHandler municipalityCreateHandler,
            IMunicipalityUpdateHandler municipalityUpdateHandler,
            IMunicipalityDeleteHandler municipalityDeleteHandler,
            IMunicipalityGetSingleHandler municipalityGetSingleHandler,
            IMunicipalityGetAllHandler municipalityGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipalityCreateHandler = municipalityCreateHandler;
            _municipalityCreateHandler.NotNull(nameof(municipalityCreateHandler));

            _municipalityUpdateHandler = municipalityUpdateHandler;
            _municipalityUpdateHandler.NotNull(nameof(municipalityUpdateHandler));

            _municipalityDeleteHandler = municipalityDeleteHandler;
            _municipalityDeleteHandler.NotNull(nameof(municipalityDeleteHandler));

            _municipalityGetSingleHandler = municipalityGetSingleHandler;
            _municipalityGetSingleHandler.NotNull(nameof(municipalityGetSingleHandler));

            _municipalityGetAllHandler = municipalityGetAllHandler;
            _municipalityGetAllHandler.NotNull(nameof(municipalityGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MunicipalityCreateDto createDto , CancellationToken cancellationToken)
        {
            await _municipalityCreateHandler.Handle(createDto, cancellationToken);  
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityUpdateDto>), StatusCodes.Status200OK)]

        public async Task<IActionResult> Update([FromBody] MunicipalityUpdateDto updateDto , CancellationToken cancellationToken)
        {
            await _municipalityUpdateHandler.Handel(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MunicipalityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _municipalityDeleteHandler.Handel(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            MunicipalityGetDto municipality = await _municipalityGetSingleHandler.Handle(id, cancellationToken);
            return Ok(municipality);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MunicipalityGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MunicipalityGetDto> municipality = await _municipalityGetAllHandler.Handle(cancellationToken);
            return Ok(municipality);
        }
    }
}
