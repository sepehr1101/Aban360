using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/endpoint")]
    public class EndpointGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEndpointGetAllHandler _endpointGetAllHandler;
        public EndpointGetAllController(
            IUnitOfWork uow,
            IEndpointGetAllHandler endpointGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endpointGetAllHandler = endpointGetAllHandler;
            _endpointGetAllHandler.NotNull(nameof(endpointGetAllHandler));
        }

        [HttpGet,HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EndpointGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var endpoint = await _endpointGetAllHandler.Handle(cancellationToken);
            return Ok(endpoint);
        }
    }

}
