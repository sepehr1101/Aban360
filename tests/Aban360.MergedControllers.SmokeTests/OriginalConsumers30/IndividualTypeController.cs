// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.People;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-type")]
    public class IndividualTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTypeCreateHandler _individualTypeCreateHandler;
        private readonly IIndividualTypeUpdateHandler _individualTypeUpdateHandler;
        private readonly IIndividualTypeDeleteHandler _individualTypeDeleteHandler;
        private readonly IIndividualTypeGetSingleHandler _individualTypeGetSingleHandler;
        private readonly IIndividualTypeGetAllHandler _individualTypeGetAllHandler;

        public IndividualTypeController(
            IUnitOfWork uow,
            IIndividualTypeCreateHandler individualTypeCreateHandler,
            IIndividualTypeUpdateHandler individualTypeUpdateHandler,
            IIndividualTypeDeleteHandler individualTypeDeleteHandler,
            IIndividualTypeGetSingleHandler individualTypeGetSingleHandler,
            IIndividualTypeGetAllHandler individualTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTypeCreateHandler = individualTypeCreateHandler;
            _individualTypeCreateHandler.NotNull(nameof(individualTypeCreateHandler));

            _individualTypeUpdateHandler = individualTypeUpdateHandler;
            _individualTypeUpdateHandler.NotNull(nameof(individualTypeUpdateHandler));

            _individualTypeDeleteHandler = individualTypeDeleteHandler;
            _individualTypeDeleteHandler.NotNull(nameof(individualTypeDeleteHandler));

            _individualTypeGetSingleHandler = individualTypeGetSingleHandler;
            _individualTypeGetSingleHandler.NotNull(nameof(individualTypeGetSingleHandler));

            _individualTypeGetAllHandler = individualTypeGetAllHandler;
            _individualTypeGetAllHandler.NotNull(nameof(individualTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            IndividualTypeGetDto IndividualType = await _individualTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(IndividualType);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualTypeGetDto> IndividualType = await _individualTypeGetAllHandler.Handle(cancellationToken);
            return Ok(IndividualType);
        }
    }
}
