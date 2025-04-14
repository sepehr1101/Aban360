using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/_calculation")]
    public class _CalculationCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly I_CalculationCreateHandler _i_CalculationCreateHandler;
        public _CalculationCreateController(
            IUnitOfWork uow,
            I_CalculationCreateHandler i_CalculationCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _i_CalculationCreateHandler = i_CalculationCreateHandler;
            _i_CalculationCreateHandler.NotNull(nameof(i_CalculationCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(string billId, string counterNumber, CancellationToken cancellationToken)
        {
            await _i_CalculationCreateHandler.Handle(billId, counterNumber, cancellationToken);
            return Ok();
        }
    }
}
