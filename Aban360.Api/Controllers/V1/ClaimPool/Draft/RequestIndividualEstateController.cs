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
    [Route("v1/request-individual-estate")]
    public class RequestIndividualEstateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualEstateCreateHandler _requestIndividualEstateCreateHandler;
        private readonly IRequestIndividualEstateUpdateHandler _requestIndividualEstateUpdateHandler;
        private readonly IRequestIndividualEstateDeleteHandler _requestIndividualEstateDeleteHandler;

        public RequestIndividualEstateController(
            IUnitOfWork uow,
            IRequestIndividualEstateCreateHandler requestIndividualEstateCreateHandler,
            IRequestIndividualEstateUpdateHandler requestIndividualEstateUpdateHandler,
            IRequestIndividualEstateDeleteHandler requestIndividualEstateDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualEstateCreateHandler = requestIndividualEstateCreateHandler;
            _requestIndividualEstateCreateHandler.NotNull(nameof(requestIndividualEstateCreateHandler));

            _requestIndividualEstateUpdateHandler = requestIndividualEstateUpdateHandler;
            _requestIndividualEstateUpdateHandler.NotNull(nameof(requestIndividualEstateUpdateHandler));

            _requestIndividualEstateDeleteHandler = requestIndividualEstateDeleteHandler;
            _requestIndividualEstateDeleteHandler.NotNull(nameof(requestIndividualEstateDeleteHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualEstateCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualEstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualEstateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualEstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualEstateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
