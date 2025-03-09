using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-material")]
    public class MeterMaterialCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterMaterialCreateHandler _meterMaterialHandler;
        public MeterMaterialCreateController(
            IUnitOfWork uow,
            IMeterMaterialCreateHandler meterMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterialHandler = meterMaterialHandler;
            _meterMaterialHandler.NotNull(nameof(meterMaterialHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] MeterMaterialCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterMaterialHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
