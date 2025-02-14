using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual")]
    public class IndividualGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualGetAllHandler _individualHandler;
        public IndividualGetAllController(
            IUnitOfWork uow,
            IIndividualGetAllHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualHandler = individualHandler;
            _individualHandler.NotNull(nameof(individualHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var individual = await _individualHandler.Handle(cancellationToken);
            return Ok(individual);
        }
    }
}
