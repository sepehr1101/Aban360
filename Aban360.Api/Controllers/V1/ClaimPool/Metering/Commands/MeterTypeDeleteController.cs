using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-type")]
    public class MeterTypeDeleteController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeDeleteHandler _meterTypeHandler;
        public MeterTypeDeleteController(
            IUnitOfWork uow,
            IMeterTypeDeleteHandler meterTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));  

            _meterTypeHandler = meterTypeHandler;
            _meterTypeHandler.NotNull(nameof(meterTypeHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MeterTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterTypeHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
