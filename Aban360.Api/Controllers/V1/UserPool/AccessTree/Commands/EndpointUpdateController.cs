using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/endpoint")]
    public class EndpointUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEndpointUpdateHandler _endpointUpdateHandler;
        public EndpointUpdateController(
            IUnitOfWork uow,
            IEndpointUpdateHandler endpointUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endpointUpdateHandler = endpointUpdateHandler;
            _endpointUpdateHandler.NotNull(nameof(endpointUpdateHandler));
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EndpointUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _endpointUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }

}
