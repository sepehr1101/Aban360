using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-type")]
    public class SiphonTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonTypeGetAllHandler _siphonTypeHandler;
        public SiphonTypeGetAllController(
            IUnitOfWork uow,
            ISiphonTypeGetAllHandler siphonTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonTypeHandler = siphonTypeHandler;
            _siphonTypeHandler.NotNull(nameof(siphonTypeHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var siphonType = await _siphonTypeHandler.Handle(cancellationToken);
            return Ok(siphonType);
        }
    }
}
