using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual")]
    public class IndividualCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualCreateHandler _individualHandler;
        public IndividualCreateController(
            IUnitOfWork uow,
            IIndividualCreateHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualHandler = individualHandler;
            _individualHandler.NotNull(nameof(individualHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] IndividualCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
