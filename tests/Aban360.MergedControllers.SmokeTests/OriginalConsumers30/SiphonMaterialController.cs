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
    [Route("v1/siphon-material")]
    public class SiphonMaterialController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonMaterialCreateHandler _siphonMaterialCreateHandler;
        private readonly ISiphonMaterialUpdateHandler _siphonMaterialUpdateHandler;
        private readonly ISiphonMaterialDeleteHandler _siphonMaterialDeleteHandler;
        private readonly ISiphonMaterialGetSingleHandler _siphonMaterialGetSingleHandler;
        private readonly ISiphonMaterialGetAllHandler _siphonMaterialGetAllHandler;

        public SiphonMaterialController(
            IUnitOfWork uow,
            ISiphonMaterialCreateHandler siphonMaterialCreateHandler,
            ISiphonMaterialUpdateHandler siphonMaterialUpdateHandler,
            ISiphonMaterialDeleteHandler siphonMaterialDeleteHandler,
            ISiphonMaterialGetSingleHandler siphonMaterialGetSingleHandler,
            ISiphonMaterialGetAllHandler siphonMaterialGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonMaterialCreateHandler = siphonMaterialCreateHandler;
            _siphonMaterialCreateHandler.NotNull(nameof(siphonMaterialCreateHandler));

            _siphonMaterialUpdateHandler = siphonMaterialUpdateHandler;
            _siphonMaterialUpdateHandler.NotNull(nameof(siphonMaterialUpdateHandler));

            _siphonMaterialDeleteHandler = siphonMaterialDeleteHandler;
            _siphonMaterialDeleteHandler.NotNull(nameof(siphonMaterialDeleteHandler));

            _siphonMaterialGetSingleHandler = siphonMaterialGetSingleHandler;
            _siphonMaterialGetSingleHandler.NotNull(nameof(siphonMaterialGetSingleHandler));

            _siphonMaterialGetAllHandler = siphonMaterialGetAllHandler;
            _siphonMaterialGetAllHandler.NotNull(nameof(siphonMaterialGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonMaterialCreateDto createDto, CancellationToken cancellationToken)
        {
            await _siphonMaterialCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SiphonMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _siphonMaterialUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonMaterialDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            SiphonMaterialGetDto siphonMaterial = await _siphonMaterialGetSingleHandler.Handle(id, cancellationToken);
            return Ok(siphonMaterial);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SiphonMaterialGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SiphonMaterialGetDto> siphonMaterials = await _siphonMaterialGetAllHandler.Handle(cancellationToken);
            return Ok(siphonMaterials);
        }
    }
}
