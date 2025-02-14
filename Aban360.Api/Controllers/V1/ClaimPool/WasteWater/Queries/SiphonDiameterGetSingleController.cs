using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-diameter")]
    public class SiphonDiameterGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonDiameterGetSingleHandler _siphonDiameterHandler;
        public SiphonDiameterGetSingleController(
            IUnitOfWork uow,
            ISiphonDiameterGetSingleHandler siphonDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonDiameterHandler = siphonDiameterHandler;
            _siphonDiameterHandler.NotNull(nameof(siphonDiameterHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var siphonDiameter = await _siphonDiameterHandler.Handle(id, cancellationToken);
            return Ok(siphonDiameter);
        }
    }
}
