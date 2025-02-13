using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("estate")]
    public class EstateGetSingleController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateGetSingleHandler _getSingleHandler;
        public EstateGetSingleController(
            IUnitOfWork uow,
            IEstateGetSingleHandler getSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getSingleHandler = getSingleHandler;
            _getSingleHandler.NotNull(nameof(_getSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var estate = await _getSingleHandler.Handle(id, cancellationToken);
            return Ok(estate);
        }
    }
}
