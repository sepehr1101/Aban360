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

namespace Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-discount-type")]
    public class IndividualDiscountTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualDiscountTypeCreateHandler _individualDiscountTypeCreateHandler;
        private readonly IIndividualDiscountTypeUpdateHandler _individualDiscountTypeUpdateHandler;
        private readonly IIndividualDiscountTypeDeleteHandler _individualDiscountTypeDeleteHandler;
        private readonly IIndividualDiscountTypeGetSingleHandler _individualDiscountTypeGetSingleHandler;
        private readonly IIndividualDiscountTypeGetAllHandler _individualDiscountTypeGetAllHandler;

        public IndividualDiscountTypeController(
            IUnitOfWork uow,
            IIndividualDiscountTypeCreateHandler individualDiscountTypeCreateHandler,
            IIndividualDiscountTypeUpdateHandler individualDiscountTypeUpdateHandler,
            IIndividualDiscountTypeDeleteHandler individualDiscountTypeDeleteHandler,
            IIndividualDiscountTypeGetSingleHandler individualDiscountTypeGetSingleHandler,
            IIndividualDiscountTypeGetAllHandler individualDiscountTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualDiscountTypeCreateHandler = individualDiscountTypeCreateHandler;
            _individualDiscountTypeCreateHandler.NotNull(nameof(individualDiscountTypeCreateHandler));

            _individualDiscountTypeUpdateHandler = individualDiscountTypeUpdateHandler;
            _individualDiscountTypeUpdateHandler.NotNull(nameof(individualDiscountTypeUpdateHandler));

            _individualDiscountTypeDeleteHandler = individualDiscountTypeDeleteHandler;
            _individualDiscountTypeDeleteHandler.NotNull(nameof(individualDiscountTypeDeleteHandler));

            _individualDiscountTypeGetSingleHandler = individualDiscountTypeGetSingleHandler;
            _individualDiscountTypeGetSingleHandler.NotNull(nameof(individualDiscountTypeGetSingleHandler));

            _individualDiscountTypeGetAllHandler = individualDiscountTypeGetAllHandler;
            _individualDiscountTypeGetAllHandler.NotNull(nameof(individualDiscountTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualDiscountTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualDiscountTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualDiscountTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var individualDiscountTypes = await _individualDiscountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(individualDiscountTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualDiscountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var individualDiscountTypes = await _individualDiscountTypeGetAllHandler.Handle(cancellationToken);
            return Ok(individualDiscountTypes);
        }
    }
}
