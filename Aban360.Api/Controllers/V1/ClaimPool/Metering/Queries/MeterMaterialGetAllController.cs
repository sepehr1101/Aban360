using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("meter-material")]
    public class MeterMaterialGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterMaterialGetAllHandler _meterMaterialHandler;
        public MeterMaterialGetAllController(
            IUnitOfWork uow,
            IMeterMaterialGetAllHandler meterMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterialHandler = meterMaterialHandler;
            _meterMaterialHandler.NotNull(nameof(meterMaterialHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var meterMaterial = await _meterMaterialHandler.Handle(cancellationToken);
            return Ok(meterMaterial);
        }
    }
}
