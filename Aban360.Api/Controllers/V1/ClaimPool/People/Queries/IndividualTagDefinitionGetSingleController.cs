using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-tag-definition")]
    public class IndividualTagDefinitionGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagDefinitionGetSingleHandler _tagDefinitionHandler;
        public IndividualTagDefinitionGetSingleController(
            IUnitOfWork uow,
            IIndividualTagDefinitionGetSingleHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDefinitionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var IndividualTagDefinition = await _tagDefinitionHandler.Handle(id, cancellationToken);
            return Ok(IndividualTagDefinition);
        }
    }
}
