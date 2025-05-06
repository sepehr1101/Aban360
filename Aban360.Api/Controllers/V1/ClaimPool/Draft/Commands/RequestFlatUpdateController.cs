using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-flat")]
    public class RequestFlatUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestFlatUpdateHandler _requestFlatUpdateHandler;
        public RequestFlatUpdateController(
            IUnitOfWork uow,
            IRequestFlatUpdateHandler requestFlatUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestFlatUpdateHandler = requestFlatUpdateHandler;
            _requestFlatUpdateHandler.NotNull(nameof(requestFlatUpdateHandler));
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
    }
}
