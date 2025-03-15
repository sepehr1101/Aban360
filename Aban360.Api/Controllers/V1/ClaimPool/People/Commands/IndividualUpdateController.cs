using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual")]
    public class IndividualUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualUpdateHandler _individualHandler;
        public IndividualUpdateController(
            IUnitOfWork uow,
            IIndividualUpdateHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualHandler = individualHandler;
            _individualHandler.NotNull(nameof(individualHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
