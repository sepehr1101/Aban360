using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/gateway")]
    public class GatewayUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGatewayUpdateHandler _gatewayUpdateHandler;
        public GatewayUpdateController(
            IUnitOfWork uow,
            IGatewayUpdateHandler getewayUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _gatewayUpdateHandler = getewayUpdateHandler;
            _gatewayUpdateHandler.NotNull(nameof(getewayUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] GetewayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _gatewayUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
