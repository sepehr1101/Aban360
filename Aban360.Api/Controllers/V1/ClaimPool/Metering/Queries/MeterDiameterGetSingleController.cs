using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("meter-diameter")]
    public class MeterDiameterGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterGetSingleHandler _meterDiameterHandler;
        public MeterDiameterGetSingleController(
            IUnitOfWork uow,
            IMeterDiameterGetSingleHandler meterDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterHandler = meterDiameterHandler;
            _meterDiameterHandler.NotNull(nameof(meterDiameterHandler));
        }

        [HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetAll(short id, CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterHandler.Handle(id, cancellationToken);
            return Ok(meterDiameter);
        }
    }
}
