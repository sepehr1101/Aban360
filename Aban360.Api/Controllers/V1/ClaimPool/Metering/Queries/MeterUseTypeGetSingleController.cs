using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("meter-use-type")]
    public class MeterUseTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterUseTypeGetSingleHandler _meterUseTypeHandler;
        public MeterUseTypeGetSingleController(
            IUnitOfWork uow,
            IMeterUseTypeGetSingleHandler meterUseTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterUseTypeHandler = meterUseTypeHandler;
            _meterUseTypeHandler.NotNull(nameof(meterUseTypeHandler));
        }

        [HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var meterUseType = await _meterUseTypeHandler.Handle(id, cancellationToken);
            return Ok(meterUseType);
        }
    }
}
