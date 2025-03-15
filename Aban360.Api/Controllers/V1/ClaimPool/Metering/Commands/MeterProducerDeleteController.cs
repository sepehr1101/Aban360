using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-producer")]
    public class MeterProducerDeleteController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterProducerDeleteHandler _meterProducerHandler;
        public MeterProducerDeleteController(
            IUnitOfWork uow,
            IMeterProducerDeleteHandler meterProducerHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterProducerHandler = meterProducerHandler;
            _meterProducerHandler.NotNull(nameof(meterProducerHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterProducerDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterProducerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterProducerHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
