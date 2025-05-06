using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual")]
    public class RequestIndividualUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualUpdateHandler _requestIndividualUpdateHandler;
        public RequestIndividualUpdateController(
            IUnitOfWork uow,
            IRequestIndividualUpdateHandler requestIndividualUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualUpdateHandler = requestIndividualUpdateHandler;
            _requestIndividualUpdateHandler.NotNull(nameof(requestIndividualUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
