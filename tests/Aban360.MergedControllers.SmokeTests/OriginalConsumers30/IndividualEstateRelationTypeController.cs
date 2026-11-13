// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.People;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-estate-relation-type")]
    public class IndividualEstateRelationTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateRelationTypeCreateHandler _individualEstateRelationTypeCreateHandler;
        private readonly IIndividualEstateRelationTypeUpdateHandler _individualEstateRelationTypeUpdateHandler;
        private readonly IIndividualEstateRelationTypeDeleteHandler _individualEstateRelationTypeDeleteHandler;
        private readonly IIndividualEstateRelationTypeGetSingelHandler _individualEstateRelationTypeGetSingelHandler;
        private readonly IIndividualEstateRelationTypeGetAllHandler _individualEstateRelationTypeGetAllHandler;

        public IndividualEstateRelationTypeController(
            IUnitOfWork uow,
            IIndividualEstateRelationTypeCreateHandler individualEstateRelationTypeCreateHandler,
            IIndividualEstateRelationTypeUpdateHandler individualEstateRelationTypeUpdateHandler,
            IIndividualEstateRelationTypeDeleteHandler individualEstateRelationTypeDeleteHandler,
            IIndividualEstateRelationTypeGetSingelHandler individualEstateRelationTypeGetSingelHandler,
            IIndividualEstateRelationTypeGetAllHandler individualEstateRelationTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateRelationTypeCreateHandler = individualEstateRelationTypeCreateHandler;
            _individualEstateRelationTypeCreateHandler.NotNull(nameof(individualEstateRelationTypeCreateHandler));

            _individualEstateRelationTypeUpdateHandler = individualEstateRelationTypeUpdateHandler;
            _individualEstateRelationTypeUpdateHandler.NotNull(nameof(individualEstateRelationTypeUpdateHandler));

            _individualEstateRelationTypeDeleteHandler = individualEstateRelationTypeDeleteHandler;
            _individualEstateRelationTypeDeleteHandler.NotNull(nameof(individualEstateRelationTypeDeleteHandler));

            _individualEstateRelationTypeGetSingelHandler = individualEstateRelationTypeGetSingelHandler;
            _individualEstateRelationTypeGetSingelHandler.NotNull(nameof(individualEstateRelationTypeGetSingelHandler));

            _individualEstateRelationTypeGetAllHandler = individualEstateRelationTypeGetAllHandler;
            _individualEstateRelationTypeGetAllHandler.NotNull(nameof(individualEstateRelationTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRelationTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualEstateRelationTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualEstateRelationTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRelationTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualEstateRelationTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualEstateRelationTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRelationTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualEstateRelationTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualEstateRelationTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRelationTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle (IndividualEstateRelationTypeEnum id,CancellationToken cancellationToken)
        {
            IndividualEstateRelationTypeGetDto individualEstateRelationType = await _individualEstateRelationTypeGetSingelHandler.Handle(id, cancellationToken);
            return Ok(individualEstateRelationType);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualEstateRelationTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualEstateRelationTypeGetDto> individualEstateRelationTypes = await _individualEstateRelationTypeGetAllHandler.Handle(cancellationToken);
            return Ok(individualEstateRelationTypes);
        }
    }
}
