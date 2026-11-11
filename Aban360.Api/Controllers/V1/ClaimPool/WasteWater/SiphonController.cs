using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater
{
    [Route("v1/siphon")]
    public class SiphonController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonCreateHandler _siphonCreateHandler;
        private readonly ISiphonUpdateHandler _siphonUpdateHandler;
        private readonly ISiphonDeleteHandler _siphonDeleteHandler;
        private readonly ISiphonGetSingleHandler _siphonGetSingleHandler;
        private readonly ISiphonGetAllHandler _siphonGetAllHandler;

        public SiphonController(
            IUnitOfWork uow,
            ISiphonCreateHandler siphonCreateHandler,
            ISiphonUpdateHandler siphonUpdateHandler,
            ISiphonDeleteHandler siphonDeleteHandler,
            ISiphonGetSingleHandler siphonGetSingleHandler,
            ISiphonGetAllHandler siphonGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonCreateHandler = siphonCreateHandler;
            _siphonCreateHandler.NotNull(nameof(siphonCreateHandler));

            _siphonUpdateHandler = siphonUpdateHandler;
            _siphonUpdateHandler.NotNull(nameof(siphonUpdateHandler));

            _siphonDeleteHandler = siphonDeleteHandler;
            _siphonDeleteHandler.NotNull(nameof(siphonDeleteHandler));

            _siphonGetSingleHandler = siphonGetSingleHandler;
            _siphonGetSingleHandler.NotNull(nameof(siphonGetSingleHandler));

            _siphonGetAllHandler = siphonGetAllHandler;
            _siphonGetAllHandler.NotNull(nameof(siphonGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonCreateDto createDto, CancellationToken cancellationToken)
        {
            await _siphonCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SiphonUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _siphonUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            SiphonGetDto siphon = await _siphonGetSingleHandler.Handle(id, cancellationToken);
            return Ok(siphon);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SiphonGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SiphonGetDto> siphon = await _siphonGetAllHandler.Handle(cancellationToken);
            return Ok(siphon);
        }
    }
}
