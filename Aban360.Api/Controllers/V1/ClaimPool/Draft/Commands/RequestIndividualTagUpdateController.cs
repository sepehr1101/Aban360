using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual-tag")]
    public class RequestIndividualTagUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualTagUpdateHandler _requestIndividualTagUpdateHandler;
        public RequestIndividualTagUpdateController(
            IUnitOfWork uow,
            IRequestIndividualTagUpdateHandler requestIndividualTagUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualTagUpdateHandler = requestIndividualTagUpdateHandler;
            _requestIndividualTagUpdateHandler.NotNull(nameof(requestIndividualTagUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualTagUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
