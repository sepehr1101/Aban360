using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualEstateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualEstateGetDto> individualEstateHandler = await _individualEstateHandler.Handle(cancellationToken);
            return Ok(individualEstateHandler);
        }
    }
}