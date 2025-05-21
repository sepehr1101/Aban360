using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-estate")]
    public class RequestEstateUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestEstateUpdateHandler _requestEstateUpdateHandler;
        public RequestEstateUpdateController(
            IUnitOfWork uow,
            IRequestEstateUpdateHandler requestEstateUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestEstateUpdateHandler = requestEstateUpdateHandler;
            _requestEstateUpdateHandler.NotNull(nameof(requestEstateUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestEstateUpdateHandler.Handle(CurrentUser,updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
