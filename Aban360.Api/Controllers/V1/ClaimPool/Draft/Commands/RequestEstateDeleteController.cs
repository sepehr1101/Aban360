using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-estate")]
    public class RequestEstateDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestEstateDeleteHandler _requestEstateDeleteHandler;
        public RequestEstateDeleteController(
            IUnitOfWork uow,
            IRequestEstateDeleteHandler requestEstateDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestEstateDeleteHandler = requestEstateDeleteHandler;
            _requestEstateDeleteHandler.NotNull(nameof(requestEstateDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestEstateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
