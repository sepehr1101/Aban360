using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-diameter")]
    public class MeterDiameterUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterUpdateHandler _meterDiameterHandler;
        public MeterDiameterUpdateController(
            IUnitOfWork uow,
            IMeterDiameterUpdateHandler meterDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterHandler = meterDiameterHandler;
            _meterDiameterHandler.NotNull(nameof(meterDiameterHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] MeterDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterDiameterHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
