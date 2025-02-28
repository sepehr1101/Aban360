using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/endpoint")]
    public class EndpointCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEndpointCreateHandler _endpointCreateHandler;
        public EndpointCreateController(
            IUnitOfWork uow,
            IEndpointCreateHandler endpointCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endpointCreateHandler = endpointCreateHandler;
            _endpointCreateHandler.NotNull(nameof(endpointCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EndpointCreateDto createDto, CancellationToken cancellationToken)
        {
            await _endpointCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
