using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("individual-tag")]
    public class IndividualTagGetSingleBySearchInputController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagGetSinglBySearchInputeHandler _individualTagHandler;
        public IndividualTagGetSingleBySearchInputController(
            IUnitOfWork uow,
            IIndividualTagGetSinglBySearchInputeHandler individualTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTagHandler = individualTagHandler;
            _individualTagHandler.NotNull(nameof(individualTagHandler));
        }

        [HttpPost, HttpGet]
        [Route("search/{id}")]
        public async Task<IActionResult> Search(string input, CancellationToken cancellationToken)
        {
            var IndividualTag = await _individualTagHandler.Handle(input, cancellationToken);
            return Ok(IndividualTag);
        }
    }
}
