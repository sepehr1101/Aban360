using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/endpoint")]
    public class EndpointGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEndpointGetSingleHandler _endpointGetSingleHandler;
        public EndpointGetSingleController(
            IUnitOfWork uow,
            IEndpointGetSingleHandler endpointGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endpointGetSingleHandler = endpointGetSingleHandler;
            _endpointGetSingleHandler.NotNull(nameof(endpointGetSingleHandler));
        }

        [HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointGetSingleHandler.Handle(id, cancellationToken);
            return Ok(endpoint);
        }
    }

}
