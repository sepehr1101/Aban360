using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon")]
    public class SiphonGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonGetAllHandler _siphonHandler;
        public SiphonGetAllController(
            IUnitOfWork uow,
            ISiphonGetAllHandler siphonHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonHandler = siphonHandler;
            _siphonHandler.NotNull(nameof(siphonHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var siphon = await _siphonHandler.Handle(cancellationToken);
            return Ok(siphon);
        }
    }
}
