using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/executable-mimetype")]
    public class ExecutableMimetypeGetSingleController : BaseController
    {
        private readonly IExecutableMimetypeGetSingleHandler _executableMimetypeGetSingleHandler;
        public ExecutableMimetypeGetSingleController(IExecutableMimetypeGetSingleHandler executableMimetypeGetSingleHandler)
        {
            _executableMimetypeGetSingleHandler = executableMimetypeGetSingleHandler;
            _executableMimetypeGetSingleHandler.NotNull(nameof(executableMimetypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var executableMimetypes = await _executableMimetypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(executableMimetypes);
        }
    }
}
