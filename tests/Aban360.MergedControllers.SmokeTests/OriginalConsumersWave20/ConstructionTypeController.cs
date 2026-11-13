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
    [Route("v1/construction-type")]
    public class ConstructionTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConstructionTypeCreateHandler _constructionTypeCreateHandler;
        private readonly IConstructionTypeUpdateHandler _constructionTypeUpdateHandler;
        private readonly IConstructionTypeDeleteHandler _constructionTypeDeleteHandler;
        private readonly IConstructionTypeGetSingleHandler _constructionTypeGetSingleHandler;
        private readonly IConstructionTypeGetAllHandler _constructionTypeGetAllHandler;

        public ConstructionTypeController(
            IUnitOfWork uow,
            IConstructionTypeCreateHandler constructionTypeCreateHandler,
            IConstructionTypeUpdateHandler constructionTypeUpdateHandler,
            IConstructionTypeDeleteHandler constructionTypeDeleteHandler,
            IConstructionTypeGetSingleHandler constructionTypeGetSingleHandler,
            IConstructionTypeGetAllHandler constructionTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _constructionTypeCreateHandler = constructionTypeCreateHandler;
            _constructionTypeCreateHandler.NotNull(nameof(_constructionTypeCreateHandler));

            _constructionTypeUpdateHandler = constructionTypeUpdateHandler;
            _constructionTypeUpdateHandler.NotNull(nameof(_constructionTypeUpdateHandler));

            _constructionTypeDeleteHandler = constructionTypeDeleteHandler;
            _constructionTypeDeleteHandler.NotNull(nameof(_constructionTypeDeleteHandler));

            _constructionTypeGetSingleHandler = constructionTypeGetSingleHandler;
            _constructionTypeGetSingleHandler.NotNull(nameof(_constructionTypeGetSingleHandler));

            _constructionTypeGetAllHandler = constructionTypeGetAllHandler;
            _constructionTypeGetAllHandler.NotNull(nameof(_constructionTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConstructionTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ConstructionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _constructionTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConstructionTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ConstructionTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _constructionTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConstructionTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ConstructionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _constructionTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConstructionTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ConstructionTypeGetDto result = await _constructionTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ConstructionTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ConstructionTypeGetDto> result = await _constructionTypeGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
