using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-diameter")]
    public class MeterDiameterDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterDeleteHandler _meterDiameterHandler;
        public MeterDiameterDeleteController(
            IUnitOfWork uow,
            IMeterDiameterDeleteHandler meterDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterHandler = meterDiameterHandler;
            _meterDiameterHandler.NotNull(nameof(meterDiameterHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterDiameterHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
