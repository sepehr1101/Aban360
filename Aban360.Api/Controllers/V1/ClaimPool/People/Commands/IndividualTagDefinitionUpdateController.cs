using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("individual-tag-definition")]
    public class IndividualTagDefinitionUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagDefinitionUpdateHandler _tagDefinitionHandler;
        public IndividualTagDefinitionUpdateController(
            IUnitOfWork uow,
            IIndividualTagDefinitionUpdateHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tagDefinitionHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
