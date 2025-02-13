using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("meter-material")]
    public class MeterMaterialGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterMaterialGetSingleHandler _meterMaterialHandler;
        public MeterMaterialGetSingleController(
            IUnitOfWork uow,
            IMeterMaterialGetSingleHandler meterMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterialHandler = meterMaterialHandler;
            _meterMaterialHandler.NotNull(nameof(meterMaterialHandler));
        }

        [HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var meterMaterial = await _meterMaterialHandler.Handle(id, cancellationToken);
            return Ok(meterMaterial);
        }
    }
}
