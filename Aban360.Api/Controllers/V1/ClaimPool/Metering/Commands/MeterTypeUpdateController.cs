using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-type")]
    public class MeterTypeUpdateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeUpdateHandler _meterTypeHandler;
        public MeterTypeUpdateController(
            IUnitOfWork uow,
            IMeterTypeUpdateHandler meterTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));  

            _meterTypeHandler = meterTypeHandler;
            _meterTypeHandler.NotNull(nameof(meterTypeHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] MeterTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterTypeHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
