using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-individual-discount-type")]
    public class RequestIndividualDiscountTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualDiscountTypeCreateHandler _requestIndividualDiscountTypeCreateHandler;
        private readonly IRequestIndividualDiscountTypeUpdateHandler _requestIndividualDiscountTypeUpdateHandler;
        private readonly IRequestIndividualDiscountTypeDeleteHandler _requestIndividualDiscountTypeDeleteHandler;
        private readonly IRequestIndividualDiscountTypeGetSingleHandler _requestIndividualDiscountTypeGetSingleHandler;
        private readonly IRequestIndividualDiscountTypeGetAllHandler _requestIndividualDiscountTypeGetAllHandler;

        public RequestIndividualDiscountTypeController(
            IUnitOfWork uow,
            IRequestIndividualDiscountTypeCreateHandler requestIndividualDiscountTypeCreateHandler,
            IRequestIndividualDiscountTypeUpdateHandler requestIndividualDiscountTypeUpdateHandler,
            IRequestIndividualDiscountTypeDeleteHandler requestIndividualDiscountTypeDeleteHandler,
            IRequestIndividualDiscountTypeGetSingleHandler requestIndividualDiscountTypeGetSingleHandler,
            IRequestIndividualDiscountTypeGetAllHandler requestIndividualDiscountTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualDiscountTypeCreateHandler = requestIndividualDiscountTypeCreateHandler;
            _requestIndividualDiscountTypeCreateHandler.NotNull(nameof(requestIndividualDiscountTypeCreateHandler));

            _requestIndividualDiscountTypeUpdateHandler = requestIndividualDiscountTypeUpdateHandler;
            _requestIndividualDiscountTypeUpdateHandler.NotNull(nameof(requestIndividualDiscountTypeUpdateHandler));

            _requestIndividualDiscountTypeDeleteHandler = requestIndividualDiscountTypeDeleteHandler;
            _requestIndividualDiscountTypeDeleteHandler.NotNull(nameof(requestIndividualDiscountTypeDeleteHandler));

            _requestIndividualDiscountTypeGetSingleHandler = requestIndividualDiscountTypeGetSingleHandler;
            _requestIndividualDiscountTypeGetSingleHandler.NotNull(nameof(requestIndividualDiscountTypeGetSingleHandler));

            _requestIndividualDiscountTypeGetAllHandler = requestIndividualDiscountTypeGetAllHandler;
            _requestIndividualDiscountTypeGetAllHandler.NotNull(nameof(requestIndividualDiscountTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDiscountTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RequestIndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDiscountTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RequestIndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDiscountTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var requestIndividualDiscountTypes = await _requestIndividualDiscountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(requestIndividualDiscountTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestIndividualDiscountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var requestIndividualDiscountTypes = await _requestIndividualDiscountTypeGetAllHandler.Handle(cancellationToken);
            return Ok(requestIndividualDiscountTypes);
        }
    }
}
