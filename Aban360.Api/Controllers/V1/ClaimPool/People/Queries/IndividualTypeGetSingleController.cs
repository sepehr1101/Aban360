using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-type")]
    public class IndividualTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTypeGetSingleHandler _individualTypeHandler;
        public IndividualTypeGetSingleController(
            IUnitOfWork uow,
            IIndividualTypeGetSingleHandler individualTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTypeHandler = individualTypeHandler;
            _individualTypeHandler.NotNull(nameof(individualTypeHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> Create(short id, CancellationToken cancellationToken)
        {
            var IndividualType = await _individualTypeHandler.Handle(id, cancellationToken);
            return Ok(IndividualType);
        }
    }
}
