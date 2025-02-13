using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/meter-type")]
    public class MeterTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeGetSingleHandler _meterTypeHandler;
        public MeterTypeGetSingleController(
            IUnitOfWork uow,
            IMeterTypeGetSingleHandler meterTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterTypeHandler = meterTypeHandler;
            _meterTypeHandler.NotNull(nameof(meterTypeHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var meterType = await _meterTypeHandler.Handle(id, cancellationToken);
            return Ok(meterType);
        }
    }
}
