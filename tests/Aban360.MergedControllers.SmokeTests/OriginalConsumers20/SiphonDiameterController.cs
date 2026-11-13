// Frozen original consumer; test-only.
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

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.ClaimPool.WasteWater
{
    [Route("v1/siphon-diameter")]
    public class SiphonDiameterController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonDiameterCreateHandler _siphonDiameterCreateHandler;
        private readonly ISiphonDiameterUpdateHandler _siphonDiameterUpdateHandler;
        private readonly ISiphonDiameterDeleteHandler _siphonDiameterDeleteHandler;
        private readonly ISiphonDiameterGetSingleHandler _siphonDiameterGetSingleHandler;
        private readonly ISiphonDiameterGetAllHandler _siphonDiameterGetAllHandler;

        public SiphonDiameterController(
            IUnitOfWork uow,
            ISiphonDiameterCreateHandler siphonDiameterCreateHandler,
            ISiphonDiameterUpdateHandler siphonDiameterUpdateHandler,
            ISiphonDiameterDeleteHandler siphonDiameterDeleteHandler,
            ISiphonDiameterGetSingleHandler siphonDiameterGetSingleHandler,
            ISiphonDiameterGetAllHandler siphonDiameterGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonDiameterCreateHandler = siphonDiameterCreateHandler;
            _siphonDiameterCreateHandler.NotNull(nameof(siphonDiameterCreateHandler));

            _siphonDiameterUpdateHandler = siphonDiameterUpdateHandler;
            _siphonDiameterUpdateHandler.NotNull(nameof(siphonDiameterUpdateHandler));

            _siphonDiameterDeleteHandler = siphonDiameterDeleteHandler;
            _siphonDiameterDeleteHandler.NotNull(nameof(siphonDiameterDeleteHandler));

            _siphonDiameterGetSingleHandler = siphonDiameterGetSingleHandler;
            _siphonDiameterGetSingleHandler.NotNull(nameof(siphonDiameterGetSingleHandler));

            _siphonDiameterGetAllHandler = siphonDiameterGetAllHandler;
            _siphonDiameterGetAllHandler.NotNull(nameof(siphonDiameterGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDiameterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _siphonDiameterCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDiameterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SiphonDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _siphonDiameterUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDiameterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonDiameterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDiameterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            SiphonDiameterGetDto siphonDiameter = await _siphonDiameterGetSingleHandler.Handle(id, cancellationToken);
            return Ok(siphonDiameter);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SiphonDiameterGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SiphonDiameterGetDto> siphonDiameter = await _siphonDiameterGetAllHandler.Handle(cancellationToken);
            return Ok(siphonDiameter);
        }
    }
}
