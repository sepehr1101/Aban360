using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-estate")]
    public class IndividualEstateGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualEstateGetAllHandler _individualEstateHandler;
        public IndividualEstateGetAllController(
            IUnitOfWork uow,
            IIndividualEstateGetAllHandler individualHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualEstateHandler = individualHandler;
            _individualEstateHandler.NotNull(nameof(individualHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var individualEstateHandler = await _individualEstateHandler.Handle(cancellationToken);
            return Ok(individualEstateHandler);
        }
    }
}