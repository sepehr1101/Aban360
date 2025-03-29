using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/gateway")]
    public class GatewayCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGatewayCreateHandler _gatewayCreateHandler;
        public GatewayCreateController(
            IUnitOfWork uow,
            IGatewayCreateHandler getewayCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _gatewayCreateHandler = getewayCreateHandler;
            _gatewayCreateHandler.NotNull(nameof(getewayCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GatewayCreateDto createDto, CancellationToken cancellationToken)
        {
            await _gatewayCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
