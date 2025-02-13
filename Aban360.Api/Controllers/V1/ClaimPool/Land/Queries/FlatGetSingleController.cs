using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("falt")]
    public class FlatGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatGetSingleHandler _flatHandler;
        public FlatGetSingleController(
            IUnitOfWork uow,
            IFlatGetSingleHandler flatHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flatHandler = flatHandler;
            _flatHandler.NotNull(nameof(flatHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var flat = await _flatHandler.Handle(id, cancellationToken);
            return Ok(flat);
        }
    }
}
