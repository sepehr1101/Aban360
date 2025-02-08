using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/endpoint")]
    public class EndpointDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEndpointDeleteHandler _endpointDeleteHandler;
        public EndpointDeleteController(
            IUnitOfWork uow,
            IEndpointDeleteHandler endpointDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endpointDeleteHandler = endpointDeleteHandler;
            _endpointDeleteHandler.NotNull(nameof(endpointDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EndpointDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _endpointDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }

}
