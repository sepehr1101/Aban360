using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-material")]
    public class MeterMaterialUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterMaterialUpdateHandler _meterMaterialHandler;
        public MeterMaterialUpdateController(
            IUnitOfWork uow,
            IMeterMaterialUpdateHandler meterMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterialHandler = meterMaterialHandler;
            _meterMaterialHandler.NotNull(nameof(meterMaterialHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterMaterialHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
