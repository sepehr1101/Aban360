using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-material")]
    public class MeterMaterialDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterMaterialDeleteHandler _meterMaterialHandler;
        public MeterMaterialDeleteController(
            IUnitOfWork uow,
            IMeterMaterialDeleteHandler meterMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterialHandler = meterMaterialHandler;
            _meterMaterialHandler.NotNull(nameof(meterMaterialHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterMaterialDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterMaterialHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
