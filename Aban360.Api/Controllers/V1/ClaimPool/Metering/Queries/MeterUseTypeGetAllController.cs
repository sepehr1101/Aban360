using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-use-type")]
    public class MeterUseTypeGetAllController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterUseTypeGetAllHandler _meterUseTypeHandler;
        public MeterUseTypeGetAllController(
            IUnitOfWork uow,
            IMeterUseTypeGetAllHandler meterUseTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterUseTypeHandler = meterUseTypeHandler;
            _meterUseTypeHandler.NotNull(nameof(meterUseTypeHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var meterUseType=await _meterUseTypeHandler.Handle(cancellationToken);
            return Ok(meterUseType);
        }
    }
}
