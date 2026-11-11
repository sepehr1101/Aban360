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
using System.Runtime.CompilerServices;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionCreateHandler _cordinalDirectionCreateHandler;
        private readonly ICordinalDirectionUpdateHandler _cordinalDirectionUpdateHandler;
        private readonly ICordinalDirectionDeleteHandler _cordinalDirectionDeleteHandler;
        private readonly ICordinalDirectionGetSingleService _cordinalDirectionGetSingleService;
        private readonly ICordinalDirectionGetAllHandler _cordinalDirectionGetAllHandler;

        public CordinalDirectionController(
            IUnitOfWork uow,
            ICordinalDirectionCreateHandler cordinalDirectionCreateHandler,
            ICordinalDirectionUpdateHandler cordinalDirectionUpdateHandler,
            ICordinalDirectionDeleteHandler cordinalDirectionDeleteHandler,
            ICordinalDirectionGetSingleService cordinalDirectionGetSingleService,
            ICordinalDirectionGetAllHandler cordinalDirectionGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _cordinalDirectionCreateHandler = cordinalDirectionCreateHandler;
            _cordinalDirectionCreateHandler.NotNull(nameof(cordinalDirectionCreateHandler));

            _cordinalDirectionUpdateHandler = cordinalDirectionUpdateHandler;
            _cordinalDirectionUpdateHandler.NotNull(nameof(cordinalDirectionUpdateHandler));

            _cordinalDirectionDeleteHandler = cordinalDirectionDeleteHandler;
            _cordinalDirectionDeleteHandler.NotNull(nameof(cordinalDirectionDeleteHandler));

            _cordinalDirectionGetSingleService = cordinalDirectionGetSingleService;
            _cordinalDirectionGetSingleService.NotNull(nameof(cordinalDirectionGetSingleService));

            _cordinalDirectionGetAllHandler = cordinalDirectionGetAllHandler;
            _cordinalDirectionGetAllHandler.NotNull(nameof(cordinalDirectionGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CordinalDirectionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _cordinalDirectionCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CordinalDirectionUpdateDto updatedto, CancellationToken cancellationToken)
        {
            await _cordinalDirectionUpdateHandler.Handle(updatedto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updatedto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _cordinalDirectionDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("Single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSigle(short id, CancellationToken cancellationToken)
        {
            CordinalDirectionGetDto cordinalDirection = await _cordinalDirectionGetSingleService.Handle(id, cancellationToken);
            return Ok(cordinalDirection);
        }

        [HttpGet, HttpPost]
        [Route("All")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CordinalDirectionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
        {
            ICollection<CordinalDirectionGetDto> cordinalDirection = await _cordinalDirectionGetAllHandler.Handle(cancellationToken);
            return Ok(cordinalDirection);
        }
    }
}
