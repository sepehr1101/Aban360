using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/counter-state")]
    public class CounterStateCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICounterStateCreateHandler _counterStateCreateHandler;
        public CounterStateCreateController(
            IUnitOfWork uow,
            ICounterStateCreateHandler counterStateCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _counterStateCreateHandler = counterStateCreateHandler;
            _counterStateCreateHandler.NotNull(nameof(counterStateCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CounterStateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _counterStateCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
