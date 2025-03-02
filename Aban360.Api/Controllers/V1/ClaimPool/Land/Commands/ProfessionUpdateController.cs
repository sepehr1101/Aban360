using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/profession")]
    public class ProfessionUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessionUpdateHandler _professionHandler;
        public ProfessionUpdateController(
            IUnitOfWork uow,
            IProfessionUpdateHandler professionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _professionHandler = professionHandler;
            _professionHandler.NotNull(nameof(professionHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProfessionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProfessionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _professionHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
