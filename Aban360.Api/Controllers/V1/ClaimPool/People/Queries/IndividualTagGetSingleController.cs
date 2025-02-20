using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/individual-tag")]
    public class IndividualTagGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagGetSingleHandler _individualTagHandler;
        public IndividualTagGetSingleController(
            IUnitOfWork uow,
            IIndividualTagGetSingleHandler individualTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTagHandler = individualTagHandler;
            _individualTagHandler.NotNull(nameof(individualTagHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var IndividualTag = await _individualTagHandler.Handle(id, cancellationToken);
            return Ok(IndividualTag);
        }
    }
}
