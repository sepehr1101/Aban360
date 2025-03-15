using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-tag")]
    public class IndividualTagUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagUpdateHandler _individualTagHandler;
        public IndividualTagUpdateController(
            IUnitOfWork uow,
            IIndividualTagUpdateHandler individualTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTagHandler = individualTagHandler;
            _individualTagHandler.NotNull(nameof(individualTagHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualTagHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
