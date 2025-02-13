using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("Estate")]
    public class EstateGetAllController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateGetAllHandler _getAllHandler;
        public EstateGetAllController(
            IUnitOfWork uow,
            IEstateGetAllHandler getAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(_getAllHandler));
        }

        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var estate = await _getAllHandler.Handle(cancellationToken);
            return Ok(estate);
        }
    }
}
