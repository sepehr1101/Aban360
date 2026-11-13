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
    [Route("v1/request-individual")]
    public class RequestIndividualController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualCreateHandler _requestIndividualCreateHandler;
        private readonly IRequestIndividualUpdateHandler _requestIndividualUpdateHandler;
        private readonly IRequestIndividualDeleteHandler _requestIndividualDeleteHandler;

        public RequestIndividualController(
            IUnitOfWork uow,
            IRequestIndividualCreateHandler requestIndividualCreateHandler,
            IRequestIndividualUpdateHandler requestIndividualUpdateHandler,
            IRequestIndividualDeleteHandler requestIndividualDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualCreateHandler = requestIndividualCreateHandler;
            _requestIndividualCreateHandler.NotNull(nameof(requestIndividualCreateHandler));

            _requestIndividualUpdateHandler = requestIndividualUpdateHandler;
            _requestIndividualUpdateHandler.NotNull(nameof(requestIndividualUpdateHandler));

            _requestIndividualDeleteHandler = requestIndividualDeleteHandler;
            _requestIndividualDeleteHandler.NotNull(nameof(requestIndividualDeleteHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualUpdateHandler.Handle(CurrentUser,updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
