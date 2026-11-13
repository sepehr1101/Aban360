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

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/profession")]
    public class ProfessionController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessionCreateHandler _professionCreateHandler;
        private readonly IProfessionUpdateHandler _professionUpdateHandler;
        private readonly IProfessionDeleteHandler _professionDeleteHandler;
        private readonly IProfessionGetSingleHandler _professionGetSingleHandler;
        private readonly IProfessionGetAllHandler _professionGetAllHandler;

        public ProfessionController(
            IUnitOfWork uow,
            IProfessionCreateHandler professionCreateHandler,
            IProfessionUpdateHandler professionUpdateHandler,
            IProfessionDeleteHandler professionDeleteHandler,
            IProfessionGetSingleHandler professionGetSingleHandler,
            IProfessionGetAllHandler professionGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _professionCreateHandler = professionCreateHandler;
            _professionCreateHandler.NotNull(nameof(professionCreateHandler));

            _professionUpdateHandler = professionUpdateHandler;
            _professionUpdateHandler.NotNull(nameof(professionUpdateHandler));

            _professionDeleteHandler = professionDeleteHandler;
            _professionDeleteHandler.NotNull(nameof(professionDeleteHandler));

            _professionGetSingleHandler = professionGetSingleHandler;
            _professionGetSingleHandler.NotNull(nameof(professionGetSingleHandler));

            _professionGetAllHandler = professionGetAllHandler;
            _professionGetAllHandler.NotNull(nameof(professionGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProfessionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProfessionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _professionCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProfessionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProfessionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _professionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProfessionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ProfessionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _professionDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProfessionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ProfessionGetDto profession = await _professionGetSingleHandler.Handle(id, cancellationToken);
            return Ok(profession);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ProfessionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ProfessionGetDto> profession = await _professionGetAllHandler.Handle(cancellationToken);
            return Ok(profession);
        }
    }
}
