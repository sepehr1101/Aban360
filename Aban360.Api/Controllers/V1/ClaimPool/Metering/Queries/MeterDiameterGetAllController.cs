using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("meter-diameter")]
    public class MeterDiameterGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterGetAllHandler _meterDiameterHandler;
        public MeterDiameterGetAllController(
            IUnitOfWork uow,
            IMeterDiameterGetAllHandler meterDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterHandler = meterDiameterHandler;
            _meterDiameterHandler.NotNull(nameof(meterDiameterHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterHandler.Handle(cancellationToken);
            return Ok(meterDiameter);
        }
    }
}
