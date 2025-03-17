using Aban360.ClaimPool.Application.Features.Draff.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draff.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draff.Commands
{
    [Route("v1/request-user")]
    public class RequestUserCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestUserCreateHandler _requestUserCreateHandler;
        public RequestUserCreateController(
            IUnitOfWork uow,
            IRequestUserCreateHandler requestUserCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestUserCreateHandler = requestUserCreateHandler;
            _requestUserCreateHandler.NotNull(nameof(requestUserCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestUserCommandDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RequestUserCommandDto createDto, CancellationToken cancellationToken)
        {
            await _requestUserCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
