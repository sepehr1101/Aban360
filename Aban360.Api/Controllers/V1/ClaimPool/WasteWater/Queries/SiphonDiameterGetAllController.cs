using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-diameter")]
    public class SiphonDiameterGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonDiameterGetAllHandler _siphonDiameterHandler;
        public SiphonDiameterGetAllController(
            IUnitOfWork uow,
            ISiphonDiameterGetAllHandler siphonDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonDiameterHandler = siphonDiameterHandler;
            _siphonDiameterHandler.NotNull(nameof(siphonDiameterHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var siphonDiameter = await _siphonDiameterHandler.Handle(cancellationToken);
            return Ok(siphonDiameter);
        }
    }
}
