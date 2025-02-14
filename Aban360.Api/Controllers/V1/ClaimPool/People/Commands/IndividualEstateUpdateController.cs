using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-estate")]
    public class IndividualEstateUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateUpdateHandler _individualEstateHandler;
        public IndividualEstateUpdateController(
            IUnitOfWork uow,
            IIndividualEstateUpdateHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateHandler = individualHandler;
            _individualEstateHandler.NotNull(nameof(individualHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] IndividualEstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualEstateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}