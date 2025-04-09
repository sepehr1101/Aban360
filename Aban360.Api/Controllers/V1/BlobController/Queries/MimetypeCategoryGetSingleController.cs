using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/mimetype_category")]
    public class MimetypeCategoryGetSingleController : BaseController
    {
        private readonly IMimetypeCategoryGetSingleHandler _mimetypeCategoryGetSingleHandler;
        public MimetypeCategoryGetSingleController(IMimetypeCategoryGetSingleHandler mimetypeCategoryGetSingleHandler)
        {
            _mimetypeCategoryGetSingleHandler = mimetypeCategoryGetSingleHandler;
            _mimetypeCategoryGetSingleHandler.NotNull(nameof(mimetypeCategoryGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MimetypeCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var mimetypeCategorys = await _mimetypeCategoryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(mimetypeCategorys);
        }
    }
}
