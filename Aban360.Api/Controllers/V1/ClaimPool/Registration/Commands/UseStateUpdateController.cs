using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Commands
{
    [Route("v1/use-state")]
    public class UseStateUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseEstateUpdateHandler _useEstateHandler;
        public UseStateUpdateController(
            IUnitOfWork uow,
            IUseEstateUpdateHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UseStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _useEstateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
