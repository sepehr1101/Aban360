using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("individual-tag")]
    public class IndividualTagDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagDeleteHandler _individualTagHandler;
        public IndividualTagDeleteController(
            IUnitOfWork uow,
            IIndividualTagDeleteHandler individualTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTagHandler = individualTagHandler;
            _individualTagHandler.NotNull(nameof(individualTagHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualTagHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
