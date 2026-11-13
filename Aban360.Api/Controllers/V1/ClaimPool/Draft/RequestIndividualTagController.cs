using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-individual-tag")]
    public class RequestIndividualTagController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualTagCreateHandler _requestIndividualTagCreateHandler;
        private readonly IRequestIndividualTagUpdateHandler _requestIndividualTagUpdateHandler;
        private readonly IRequestIndividualTagDeleteHandler _requestIndividualTagDeleteHandler;

        public RequestIndividualTagController(
            IUnitOfWork uow,
            IRequestIndividualTagCreateHandler requestIndividualTagCreateHandler,
            IRequestIndividualTagUpdateHandler requestIndividualTagUpdateHandler,
            IRequestIndividualTagDeleteHandler requestIndividualTagDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualTagCreateHandler = requestIndividualTagCreateHandler;
            _requestIndividualTagCreateHandler.NotNull(nameof(requestIndividualTagCreateHandler));

            _requestIndividualTagUpdateHandler = requestIndividualTagUpdateHandler;
            _requestIndividualTagUpdateHandler.NotNull(nameof(requestIndividualTagUpdateHandler));

            _requestIndividualTagDeleteHandler = requestIndividualTagDeleteHandler;
            _requestIndividualTagDeleteHandler.NotNull(nameof(requestIndividualTagDeleteHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualTagCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualTagUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualTagDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
