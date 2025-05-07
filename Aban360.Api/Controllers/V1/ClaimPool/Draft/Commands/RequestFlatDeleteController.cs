using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-flat")]
    public class RequestFlatDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestFlatDeleteHandler _requestFlatDeleteHandler;
        public RequestFlatDeleteController(
            IUnitOfWork uow,
            IRequestFlatDeleteHandler requestFlatDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestFlatDeleteHandler = requestFlatDeleteHandler;
            _requestFlatDeleteHandler.NotNull(nameof(requestFlatDeleteHandler));
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
