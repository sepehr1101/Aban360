using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-estate")]
    public class IndividualEstateGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateGetSingleHandler _individualEstateHandler;
        public IndividualEstateGetSingleController(
            IUnitOfWork uow,
            IIndividualEstateGetSingleHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateHandler = individualHandler;
            _individualEstateHandler.NotNull(nameof(individualHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            IndividualEstateGetDto individualEstateHandler = await _individualEstateHandler.Handle(id, cancellationToken);
            return Ok(individualEstateHandler);
        }
    }
}