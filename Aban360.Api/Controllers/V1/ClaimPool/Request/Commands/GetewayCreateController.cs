using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/geteway")]
    public class GetewayCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGetewayCreateHandler _getewayCreateHandler;
        public GetewayCreateController(
            IUnitOfWork uow,
            IGetewayCreateHandler getewayCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _getewayCreateHandler = getewayCreateHandler;
            _getewayCreateHandler.NotNull(nameof(getewayCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GetewayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GetewayCreateDto createDto, CancellationToken cancellationToken)
        {
            await _getewayCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
