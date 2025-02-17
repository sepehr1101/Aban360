using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-type")]
    public class IndividualTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTypeCreateHandler _individualTypeHandler;
        public IndividualTypeCreateController(
            IUnitOfWork uow,
            IIndividualTypeCreateHandler individualTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTypeHandler = individualTypeHandler;
            _individualTypeHandler.NotNull(nameof(individualTypeHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] IndividualTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualTypeHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
