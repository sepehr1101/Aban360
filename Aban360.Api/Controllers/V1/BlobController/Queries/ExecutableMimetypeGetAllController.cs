using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/executable-mimetype")]
    public class ExecutableMimetypeGetAllController : BaseController
    {
        private readonly IExecutableMimetypeGetAllHandler _executableMimetypeGetAllHandler;
        public ExecutableMimetypeGetAllController(IExecutableMimetypeGetAllHandler executableMimetypeGetAllHandler)
        {
            _executableMimetypeGetAllHandler = executableMimetypeGetAllHandler;
            _executableMimetypeGetAllHandler.NotNull(nameof(executableMimetypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ExecutableMimetypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var executableMimetypes = await _executableMimetypeGetAllHandler.Handle(cancellationToken);
            return Ok(executableMimetypes);
        }
    }
}
