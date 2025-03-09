using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Commands
{
    [Route("v1/tariff-constant")]
    public class TariffConstantUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffConstantUpdateHandler _tariffConstantUpdateHandler;
        public TariffConstantUpdateController(
            IUnitOfWork uow,
            ITariffConstantUpdateHandler tariffConstantUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffConstantUpdateHandler = tariffConstantUpdateHandler;
            _tariffConstantUpdateHandler.NotNull(nameof(tariffConstantUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] TariffConstantUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffConstantUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
