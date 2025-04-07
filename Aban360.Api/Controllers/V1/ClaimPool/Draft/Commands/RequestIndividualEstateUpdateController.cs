using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request_individual_estate")]
    public class RequestIndividualEstateUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualEstateUpdateHandler _requestIndividualEstateUpdateHandler;
        public RequestIndividualEstateUpdateController(
            IUnitOfWork uow,
            IRequestIndividualEstateUpdateHandler requestIndividualEstateUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualEstateUpdateHandler = requestIndividualEstateUpdateHandler;
            _requestIndividualEstateUpdateHandler.NotNull(nameof(requestIndividualEstateUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualEstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestIndividualEstateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
