using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-estate-relation-type")]
    public class IndividualEstateRelationTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateRelationTypeGetSingelHandler _individualEstateRelationTypeHandler;
        public IndividualEstateRelationTypeGetSingleController(
            IUnitOfWork uow,
            IIndividualEstateRelationTypeGetSingelHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateRelationTypeHandler = individualHandler;
            _individualEstateRelationTypeHandler.NotNull(nameof(individualHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRelationTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle (IndividualEstateRelationEnum id,CancellationToken cancellationToken)
        {
            IndividualEstateRelationTypeGetDto individualEstateRelationType = await _individualEstateRelationTypeHandler.Handle(id, cancellationToken);
            return Ok(individualEstateRelationType);
        }
    }
}
