using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-siphon")]
    public class RequestSiphonCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestSiphonCreateHandler _requestSiphonCreateHandler;
        public RequestSiphonCreateController(
            IUnitOfWork uow,
            IRequestSiphonCreateHandler requestSiphonCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestSiphonCreateHandler = requestSiphonCreateHandler;
            _requestSiphonCreateHandler.NotNull(nameof(requestSiphonCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestSiphonCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
