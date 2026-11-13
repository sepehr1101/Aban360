// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.People;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-estate")]
    public class IndividualEstateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateCreateHandler _individualEstateCreateHandler;
        private readonly IIndividualEstateUpdateHandler _individualEstateUpdateHandler;
        private readonly IIndividualEstateDeleteHandler _individualEstateDeleteHandler;
        private readonly IIndividualEstateGetSingleHandler _individualEstateGetSingleHandler;
        private readonly IIndividualEstateGetAllHandler _individualEstateGetAllHandler;

        public IndividualEstateController(
            IUnitOfWork uow,
            IIndividualEstateCreateHandler individualEstateCreateHandler,
            IIndividualEstateUpdateHandler individualEstateUpdateHandler,
            IIndividualEstateDeleteHandler individualEstateDeleteHandler,
            IIndividualEstateGetSingleHandler individualEstateGetSingleHandler,
            IIndividualEstateGetAllHandler individualEstateGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateCreateHandler = individualEstateCreateHandler;
            _individualEstateCreateHandler.NotNull(nameof(individualEstateCreateHandler));

            _individualEstateUpdateHandler = individualEstateUpdateHandler;
            _individualEstateUpdateHandler.NotNull(nameof(individualEstateUpdateHandler));

            _individualEstateDeleteHandler = individualEstateDeleteHandler;
            _individualEstateDeleteHandler.NotNull(nameof(individualEstateDeleteHandler));

            _individualEstateGetSingleHandler = individualEstateGetSingleHandler;
            _individualEstateGetSingleHandler.NotNull(nameof(individualEstateGetSingleHandler));

            _individualEstateGetAllHandler = individualEstateGetAllHandler;
            _individualEstateGetAllHandler.NotNull(nameof(individualEstateGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualEstateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualEstateCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualEstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualEstateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualEstateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualEstateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            IndividualEstateGetDto individualEstateHandler = await _individualEstateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(individualEstateHandler);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualEstateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualEstateGetDto> individualEstateHandler = await _individualEstateGetAllHandler.Handle(cancellationToken);
            return Ok(individualEstateHandler);
        }
    }
}
