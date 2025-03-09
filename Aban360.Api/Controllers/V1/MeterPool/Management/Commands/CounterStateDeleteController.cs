using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/counter-state")]
    public class CounterStateDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICounterStateDeleteHandler _counterStateDeleteHandler;
        public CounterStateDeleteController(
            IUnitOfWork uow,
            ICounterStateDeleteHandler counterStateDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _counterStateDeleteHandler = counterStateDeleteHandler;
            _counterStateDeleteHandler.NotNull(nameof(counterStateDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CounterStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _counterStateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
