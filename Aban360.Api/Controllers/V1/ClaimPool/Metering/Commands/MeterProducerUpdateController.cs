using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-producer")]
    public class MeterProducerUpdateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterProducerUpdateHandler _meterProducerHandler;
        public MeterProducerUpdateController(
            IUnitOfWork uow,
            IMeterProducerUpdateHandler meterProducerHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterProducerHandler = meterProducerHandler;
            _meterProducerHandler.NotNull(nameof(meterProducerHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] MeterProducerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterProducerHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
