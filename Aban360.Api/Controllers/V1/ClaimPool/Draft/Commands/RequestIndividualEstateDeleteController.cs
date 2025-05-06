using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual-estate")]
    public class RequestIndividualEstateDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualEstateDeleteHandler _requestIndividualEstateDeleteHandler;
        public RequestIndividualEstateDeleteController(
            IUnitOfWork uow,
            IRequestIndividualEstateDeleteHandler requestIndividualEstateDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualEstateDeleteHandler = requestIndividualEstateDeleteHandler;
            _requestIndividualEstateDeleteHandler.NotNull(nameof(requestIndividualEstateDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualEstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualEstateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
