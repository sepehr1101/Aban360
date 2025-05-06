using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-siphon")]
    public class RequestSiphonUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestSiphonUpdateHandler _requestSiphonUpdateHandler;
        public RequestSiphonUpdateController(
            IUnitOfWork uow,
            IRequestSiphonUpdateHandler requestSiphonUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestSiphonUpdateHandler = requestSiphonUpdateHandler;
            _requestSiphonUpdateHandler.NotNull(nameof(requestSiphonUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestSiphonUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
