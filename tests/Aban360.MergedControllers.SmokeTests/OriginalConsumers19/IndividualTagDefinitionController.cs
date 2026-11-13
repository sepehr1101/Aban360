using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-tag-definition")]
    public class IndividualTagDefinitionController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagDefinitionCreateHandler _individualTagDefinitionCreateHandler;
        private readonly IIndividualTagDefinitionUpdateHandler _individualTagDefinitionUpdateHandler;
        private readonly IIndividualTagDefinitionDeleteHandler _individualTagDefinitionDeleteHandler;
        private readonly IIndividualTagDefinitionGetSingleHandler _individualTagDefinitionGetSingleHandler;
        private readonly IIndividualTagDefinitionGetAllHandler _individualTagDefinitionGetAllHandler;

        public IndividualTagDefinitionController(
            IUnitOfWork uow,
            IIndividualTagDefinitionCreateHandler individualTagDefinitionCreateHandler,
            IIndividualTagDefinitionUpdateHandler individualTagDefinitionUpdateHandler,
            IIndividualTagDefinitionDeleteHandler individualTagDefinitionDeleteHandler,
            IIndividualTagDefinitionGetSingleHandler individualTagDefinitionGetSingleHandler,
            IIndividualTagDefinitionGetAllHandler individualTagDefinitionGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTagDefinitionCreateHandler = individualTagDefinitionCreateHandler;
            _individualTagDefinitionCreateHandler.NotNull(nameof(individualTagDefinitionCreateHandler));

            _individualTagDefinitionUpdateHandler = individualTagDefinitionUpdateHandler;
            _individualTagDefinitionUpdateHandler.NotNull(nameof(individualTagDefinitionUpdateHandler));

            _individualTagDefinitionDeleteHandler = individualTagDefinitionDeleteHandler;
            _individualTagDefinitionDeleteHandler.NotNull(nameof(individualTagDefinitionDeleteHandler));

            _individualTagDefinitionGetSingleHandler = individualTagDefinitionGetSingleHandler;
            _individualTagDefinitionGetSingleHandler.NotNull(nameof(individualTagDefinitionGetSingleHandler));

            _individualTagDefinitionGetAllHandler = individualTagDefinitionGetAllHandler;
            _individualTagDefinitionGetAllHandler.NotNull(nameof(individualTagDefinitionGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDefinitionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualTagDefinitionCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDefinitionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualTagDefinitionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDefinitionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualTagDefinitionDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDefinitionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            IndividualTagDefinitionGetDto IndividualTagDefinition = await _individualTagDefinitionGetSingleHandler.Handle(id, cancellationToken);
            return Ok(IndividualTagDefinition);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualTagDefinitionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualTagDefinitionGetDto> IndividualTagDefinition = await _individualTagDefinitionGetAllHandler.Handle(cancellationToken);
            return Ok(IndividualTagDefinition);
        }
    }
}
