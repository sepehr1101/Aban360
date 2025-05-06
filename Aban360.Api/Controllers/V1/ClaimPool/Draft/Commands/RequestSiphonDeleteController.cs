using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-siphon")]
    public class RequestSiphonDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestSiphonDeleteHandler _requestSiphonDeleteHandler;
        public RequestSiphonDeleteController(
            IUnitOfWork uow,
            IRequestSiphonDeleteHandler requestSiphonDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestSiphonDeleteHandler = requestSiphonDeleteHandler;
            _requestSiphonDeleteHandler.NotNull(nameof(requestSiphonDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestSiphonDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
