// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.WasteWater;
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

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.WasteWater
{
    [Route("v1/siphon-type")]
    public class SiphonTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonTypeCreateHandler _siphonTypeCreateHandler;
        private readonly ISiphonTypeUpdateHandler _siphonTypeUpdateHandler;
        private readonly ISiphonTypeDeleteHandler _siphonTypeDeleteHandler;
        private readonly ISiphonTypeGetSingleHandler _siphonTypeGetSingleHandler;
        private readonly ISiphonTypeGetAllHandler _siphonTypeGetAllHandler;

        public SiphonTypeController(
            IUnitOfWork uow,
            ISiphonTypeCreateHandler siphonTypeCreateHandler,
            ISiphonTypeUpdateHandler siphonTypeUpdateHandler,
            ISiphonTypeDeleteHandler siphonTypeDeleteHandler,
            ISiphonTypeGetSingleHandler siphonTypeGetSingleHandler,
            ISiphonTypeGetAllHandler siphonTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonTypeCreateHandler = siphonTypeCreateHandler;
            _siphonTypeCreateHandler.NotNull(nameof(siphonTypeCreateHandler));

            _siphonTypeUpdateHandler = siphonTypeUpdateHandler;
            _siphonTypeUpdateHandler.NotNull(nameof(siphonTypeUpdateHandler));

            _siphonTypeDeleteHandler = siphonTypeDeleteHandler;
            _siphonTypeDeleteHandler.NotNull(nameof(siphonTypeDeleteHandler));

            _siphonTypeGetSingleHandler = siphonTypeGetSingleHandler;
            _siphonTypeGetSingleHandler.NotNull(nameof(siphonTypeGetSingleHandler));

            _siphonTypeGetAllHandler = siphonTypeGetAllHandler;
            _siphonTypeGetAllHandler.NotNull(nameof(siphonTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _siphonTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SiphonTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _siphonTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            SiphonTypeGetDto siphonType = await _siphonTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(siphonType);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SiphonTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SiphonTypeGetDto> siphonTypes = await _siphonTypeGetAllHandler.Handle(cancellationToken);
            return Ok(siphonTypes);
        }
    }
}
