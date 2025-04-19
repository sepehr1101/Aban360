using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/handover")]
    public class HandoverCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHandoverCreateHandler _handoverCreateHandler;
        public HandoverCreateController(
            IUnitOfWork uow,
            IHandoverCreateHandler handoverCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _handoverCreateHandler = handoverCreateHandler;
            _handoverCreateHandler.NotNull(nameof(handoverCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] HandoverCreateDto createDto, CancellationToken cancellationToken)
        {
            await _handoverCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
