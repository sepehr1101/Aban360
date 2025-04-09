using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request_individual")]
    public class RequestIndividualDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualDeleteHandler _requestIndividualDeleteHandler;
        public RequestIndividualDeleteController(
            IUnitOfWork uow,
            IRequestIndividualDeleteHandler requestIndividualDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualDeleteHandler = requestIndividualDeleteHandler;
            _requestIndividualDeleteHandler.NotNull(nameof(requestIndividualDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
