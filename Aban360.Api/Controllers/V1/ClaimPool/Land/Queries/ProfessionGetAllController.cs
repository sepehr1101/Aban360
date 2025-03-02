using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/profession")]
    public class ProfessionGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessionGetAllHandler _professionHandler;
        public ProfessionGetAllController(
            IUnitOfWork uow,
            IProfessionGetAllHandler professionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _professionHandler = professionHandler;
            _professionHandler.NotNull(nameof(professionHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var profession = await _professionHandler.Handle(cancellationToken);
            return Ok(profession);
        }
    }
}
