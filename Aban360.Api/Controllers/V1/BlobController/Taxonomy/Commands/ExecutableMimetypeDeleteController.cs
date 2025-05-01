using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Taxonomy.Commands
{
    [Route("v1/executable-mimetype")]
    public class ExecutableMimetypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IExecutableMimetypeDeleteHandler _executableMimetypeDeleteHandler;
        public ExecutableMimetypeDeleteController(
            IUnitOfWork uow,
            IExecutableMimetypeDeleteHandler executableMimetypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _executableMimetypeDeleteHandler = executableMimetypeDeleteHandler;
            _executableMimetypeDeleteHandler.NotNull(nameof(executableMimetypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ExecutableMimetypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _executableMimetypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
