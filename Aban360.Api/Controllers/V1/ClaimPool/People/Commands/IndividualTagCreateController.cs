using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("individual-tag")]
    public class IndividualTagCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagCreateHandler _individualTagHandler;
        public IndividualTagCreateController(
            IUnitOfWork uow,
            IIndividualTagCreateHandler individualTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTagHandler = individualTagHandler;
            _individualTagHandler.NotNull(nameof(individualTagHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTagCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualTagHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
