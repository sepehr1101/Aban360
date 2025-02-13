using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/falt")]
    public class FlatGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatGetAllHandler _flatHandler;
        public FlatGetAllController(
            IUnitOfWork uow,
            IFlatGetAllHandler flatHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flatHandler = flatHandler;
            _flatHandler.NotNull(nameof(flatHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var flat = await _flatHandler.Handle(cancellationToken);
            return Ok(flat);
        }
    }
}
