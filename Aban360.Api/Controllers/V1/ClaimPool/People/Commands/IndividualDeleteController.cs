using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual")]
    public class IndividualDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualDeleteHandler _individualHandler;
        public IndividualDeleteController(
            IUnitOfWork uow,
            IIndividualDeleteHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualHandler = individualHandler;
            _individualHandler.NotNull(nameof(individualHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
