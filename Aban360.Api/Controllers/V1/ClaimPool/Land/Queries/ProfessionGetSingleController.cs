using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/profession")]
    public class ProfessionGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessionGetSingleHandler _professionHandler;
        public ProfessionGetSingleController(
            IUnitOfWork uow,
            IProfessionGetSingleHandler professionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _professionHandler = professionHandler;
            _professionHandler.NotNull(nameof(professionHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var profession = await _professionHandler.Handle(id, cancellationToken);
            return Ok(profession);
        }
    }
}
