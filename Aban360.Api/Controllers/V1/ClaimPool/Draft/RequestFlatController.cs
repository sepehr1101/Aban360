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
    [Route("v1/request-flat")]
    public class RequestFlatController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestFlatCreateHandler _requestFlatCreateHandler;
        private readonly IRequestFlatUpdateHandler _requestFlatUpdateHandler;
        private readonly IRequestFlatDeleteHandler _requestFlatDeleteHandler;

        public RequestFlatController(
            IUnitOfWork uow,
            IRequestFlatCreateHandler requestFlatCreateHandler,
            IRequestFlatUpdateHandler requestFlatUpdateHandler,
            IRequestFlatDeleteHandler requestFlatDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestFlatCreateHandler = requestFlatCreateHandler;
            _requestFlatCreateHandler.NotNull(nameof(requestFlatCreateHandler));

            _requestFlatUpdateHandler = requestFlatUpdateHandler;
            _requestFlatUpdateHandler.NotNull(nameof(requestFlatUpdateHandler));

            _requestFlatDeleteHandler = requestFlatDeleteHandler;
            _requestFlatDeleteHandler.NotNull(nameof(requestFlatDeleteHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FlatRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestFlatCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] FlatRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestFlatUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] FlatRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestFlatDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
