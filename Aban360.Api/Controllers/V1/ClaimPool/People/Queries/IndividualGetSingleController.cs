using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual")]
    public class IndividualGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualGetSingleHandler _individualHandler;
        public IndividualGetSingleController(
            IUnitOfWork uow,
            IIndividualGetSingleHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualHandler = individualHandler;
            _individualHandler.NotNull(nameof(individualHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            IndividualGetDto individual = await _individualHandler.Handle(id, cancellationToken);
            return Ok(individual);
        }
    }
}
