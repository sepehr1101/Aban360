using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/gateway")]
    public class GatewayDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGatewayDeleteHandler _gatewayDeleteHandler;
        public GatewayDeleteController(
            IUnitOfWork uow,
            IGatewayDeleteHandler getewayDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _gatewayDeleteHandler = getewayDeleteHandler;
            _gatewayDeleteHandler.NotNull(nameof(getewayDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GatewayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _gatewayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
