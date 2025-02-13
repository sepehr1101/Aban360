using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-use-type")]
    public class MeterUseTypeCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterUseTypeCreateHandler _meterUseTypeHandler;
        public MeterUseTypeCreateController(
            IUnitOfWork uow, 
            IMeterUseTypeCreateHandler meterUseTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterUseTypeHandler = meterUseTypeHandler;
            _meterUseTypeHandler.NotNull(nameof(meterUseTypeHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] MeterUseTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterUseTypeHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken); 

            return Ok(createDto);
        }
    }
}
