using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-tracking")]
    public class RequestTrackingCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestTrackingCreateHandler _requestTrackingCreateHandler;
        public RequestTrackingCreateController(
            IUnitOfWork uow,
            IRequestTrackingCreateHandler requestTrackingCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestTrackingCreateHandler = requestTrackingCreateHandler;
            _requestTrackingCreateHandler.NotNull(nameof(requestTrackingCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RequestTrackingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestTrackingCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
