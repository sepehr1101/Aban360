using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/executable_mimetype")]
    public class ExecutableMimetypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IExecutableMimetypeUpdateHandler _executableMimetypeUpdateHandler;
        public ExecutableMimetypeUpdateController(
            IUnitOfWork uow,
            IExecutableMimetypeUpdateHandler executableMimetypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _executableMimetypeUpdateHandler = executableMimetypeUpdateHandler;
            _executableMimetypeUpdateHandler.NotNull(nameof(executableMimetypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _executableMimetypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
