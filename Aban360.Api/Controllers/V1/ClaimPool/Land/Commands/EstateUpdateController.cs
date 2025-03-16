using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate")]
    public class EstateUpdateController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateUpdateHandler _updateHandler;
        public EstateUpdateController(
            IUnitOfWork uow,
            IEstateUpdateHandler updateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(_updateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _updateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

    }
}
