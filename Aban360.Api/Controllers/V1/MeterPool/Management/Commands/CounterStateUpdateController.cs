using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/counter-state")]
    public class CounterStateUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICounterStateUpdateHandler _counterStateUpdateHandler;
        public CounterStateUpdateController(
            IUnitOfWork uow,
            ICounterStateUpdateHandler counterStateUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _counterStateUpdateHandler = counterStateUpdateHandler;
            _counterStateUpdateHandler.NotNull(nameof(counterStateUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] CounterStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _counterStateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
