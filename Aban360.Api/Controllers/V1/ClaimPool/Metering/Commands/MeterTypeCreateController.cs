using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-type")]
    public class MeterTypeCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeCreateHandler _meterTypeHandler;
        public MeterTypeCreateController(
            IUnitOfWork uow,
            IMeterTypeCreateHandler meterTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));  

            _meterTypeHandler = meterTypeHandler;
            _meterTypeHandler.NotNull(nameof(meterTypeHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] MeterTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterTypeHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            
            return Ok(createDto);
        }
    }
}
