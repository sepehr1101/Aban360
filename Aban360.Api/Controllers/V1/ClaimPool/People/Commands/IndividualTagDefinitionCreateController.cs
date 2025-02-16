using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("individual-tag-definition")]
    public class IndividualTagDefinitionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagDefinitionCreateHandler _tagDefinitionHandler;
        public IndividualTagDefinitionCreateController(
            IUnitOfWork uow,
            IIndividualTagDefinitionCreateHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tagDefinitionHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
