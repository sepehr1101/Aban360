using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-tracking")]
    public class RequestTrackingDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestTrackingDeleteHandler _requestTrackingDeleteHandler;
        public RequestTrackingDeleteController(
            IUnitOfWork uow,
            IRequestTrackingDeleteHandler requestTrackingDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestTrackingDeleteHandler = requestTrackingDeleteHandler;
            _requestTrackingDeleteHandler.NotNull(nameof(requestTrackingDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RequestTrackingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestTrackingDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
