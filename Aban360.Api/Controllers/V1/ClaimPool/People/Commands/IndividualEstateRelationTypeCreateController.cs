using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-estate-relation-type")]
    public class IndividualEstateRelationTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateRelationTypeCreateHandler _individualEstateRelationTypeHandler;
        public IndividualEstateRelationTypeCreateController(
            IUnitOfWork uow,
            IIndividualEstateRelationTypeCreateHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateRelationTypeHandler = individualHandler;
            _individualEstateRelationTypeHandler.NotNull(nameof(individualHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRelationTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualEstateRelationTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualEstateRelationTypeHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
