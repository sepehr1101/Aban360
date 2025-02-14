using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon")]
    public class SiphonGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonGetSingleHandler _siphonHandler;
        public SiphonGetSingleController(
            IUnitOfWork uow,
            ISiphonGetSingleHandler siphonHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonHandler = siphonHandler;
            _siphonHandler.NotNull(nameof(siphonHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var siphon = await _siphonHandler.Handle(id, cancellationToken);
            return Ok(siphon);
        }
    }
}
