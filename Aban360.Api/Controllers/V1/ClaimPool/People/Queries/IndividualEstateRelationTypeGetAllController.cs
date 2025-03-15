using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-estate-relation-type")]
    public class IndividualEstateRelationTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateRelationTypeGetAllHandler _individualEstateRelationTypeHandler;
        public IndividualEstateRelationTypeGetAllController(
            IUnitOfWork uow,
            IIndividualEstateRelationTypeGetAllHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateRelationTypeHandler = individualHandler;
            _individualEstateRelationTypeHandler.NotNull(nameof(individualHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualEstateRelationTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualEstateRelationTypeGetDto> individualEstateRelationTypes = await _individualEstateRelationTypeHandler.Handle(cancellationToken);
            return Ok(individualEstateRelationTypes);
        }
    }
}
