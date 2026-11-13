using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/falt")]
    public class FlatController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatCreateHandler _flatCreateHandler;
        private readonly IFlatUpdateHandler _flatUpdateHandler;
        private readonly IFlatDeleteHandler _flatDeleteHandler;
        private readonly IFlatGetSingleHandler _flatGetSingleHandler;
        private readonly IFlatGetAllHandler _flatGetAllHandler;

        public FlatController(
            IUnitOfWork uow,
            IFlatCreateHandler flatCreateHandler,
            IFlatUpdateHandler flatUpdateHandler,
            IFlatDeleteHandler flatDeleteHandler,
            IFlatGetSingleHandler flatGetSingleHandler,
            IFlatGetAllHandler flatGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flatCreateHandler = flatCreateHandler;
            _flatCreateHandler.NotNull(nameof(flatCreateHandler));

            _flatUpdateHandler = flatUpdateHandler;
            _flatUpdateHandler.NotNull(nameof(flatUpdateHandler));

            _flatDeleteHandler = flatDeleteHandler;
            _flatDeleteHandler.NotNull(nameof(flatDeleteHandler));

            _flatGetSingleHandler = flatGetSingleHandler;
            _flatGetSingleHandler.NotNull(nameof(flatGetSingleHandler));

            _flatGetAllHandler = flatGetAllHandler;
            _flatGetAllHandler.NotNull(nameof(flatGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FlatCreateDto createDto, CancellationToken cancellationToken)
        {
            await _flatCreateHandler.Handle(createDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] FlatUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _flatUpdateHandler.Handle(updateDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] FlatDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _flatDeleteHandler.Handle(deleteDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            FlatGetDto flat = await _flatGetSingleHandler.Handle(id, cancellationToken);
            return Ok(flat);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<FlatGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<FlatGetDto> flat = await _flatGetAllHandler.Handle(cancellationToken);
            return Ok(flat);
        }
    }
}
